using System.Collections.Generic;
using UnityEngine;

public class SnakeSegments : MonoBehaviour
{
    [Header("Prefab Reference")]
    [SerializeField] private GameObject segmentPrefab;
    
    [Header("Segment Settings")]
    [SerializeField] private float segmentSpacing = 2.5f;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private int maxSegments = 6; 
    
    [SerializeField] private List<Segment> _segments = new List<Segment>();
    private List<Vector3> _positionHistory = new List<Vector3>();

    [SerializeField] private GameEvent onUpdate; 
    [SerializeField] private bool syncHealth = true; 
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    private HealthComponent _healthComponent;
    private GameManager _gameManager;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _gameManager = FindObjectOfType<GameManager>();
        if (_healthComponent == null)
        {
            Debug.LogError("SnakeSegments requires a HealthComponent!");
        }
        else
        {
            // Subscribe to damage event
            if (_healthComponent.OnDamaged != null)
                _healthComponent.OnDamaged.RegisterListener(GetComponent<GameEventListener>());
        }

        maxSegments = _gameManager.GetStatData().segmentCapacity;
    }

    private void Start()
    {
        _positionHistory.Add(transform.position);
        SyncHealthToSegments();
    }
    

    private void Update()
    {
        _positionHistory.Insert(0, transform.position);
        
        int maxHistorySize = (_segments.Count + 1) * Mathf.CeilToInt(segmentSpacing * 10);
        if (_positionHistory.Count > maxHistorySize)
        {
            _positionHistory.RemoveAt(_positionHistory.Count - 1);
        }
        
        UpdateSegmentPositions();
    }

    private void UpdateSegmentPositions()
    {
        for (int i = 0; i < _segments.Count; i++)
        {
            if (_segments[i] == null) continue;
            
            int historyIndex = Mathf.FloorToInt((i + 1) * segmentSpacing * 10);
            
            if (historyIndex < _positionHistory.Count)
            {
                Vector3 targetPosition = _positionHistory[historyIndex];
                
                _segments[i].transform.position = Vector3.Lerp(
                    _segments[i].transform.position,
                    targetPosition,
                    followSpeed * Time.deltaTime
                );
                
                if (i == 0)
                {
                    Vector3 direction = transform.position - _segments[i].transform.position;
                    if (direction != Vector3.zero)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        _segments[i].transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                    }
                }
                else
                {
                    Vector3 direction = _segments[i - 1].transform.position - _segments[i].transform.position;
                    if (direction != Vector3.zero)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        _segments[i].transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                    }
                }
            }
        }

        if (onUpdate != null)
            onUpdate.Raise();

    }

    public void AddSegment(SegmentData data)
    {
        if (_segments.Count >= maxSegments)
        {
            if (showDebugInfo)
            {
                Debug.Log($"Max segments ({maxSegments}) reached. Consuming oldest segment.");
            }
            ConsumeFirstSegment();
        }

        if (segmentPrefab == null)
        {
            Debug.LogError("Segment prefab not assigned!");
            return;
        }

        if (data == null)
        {
            Debug.LogError("Cannot add null segment data!");
            return;
        }

        Vector3 spawnPosition;
        if (_segments.Count > 0)
        {
            spawnPosition = _segments[_segments.Count - 1].transform.position;
        }
        else
        {
            spawnPosition = transform.position - transform.up * segmentSpacing;
        }

        GameObject segmentObj = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
        segmentObj.transform.parent = transform;
        
        Segment segment = segmentObj.GetComponent<Segment>();
        if (segment == null)
        {
            Debug.LogError("Segment prefab missing Segment component!");
            Destroy(segmentObj);
            return;
        }

        segment.Initialize(data);
        _segments.Add(segment);
        if (_gameManager != null)
            _gameManager.AddScore(GetSegmentCount() * _gameManager.GetStatData().waveNumber);

        SyncHealthToSegments();

        if (showDebugInfo)
        {
            Debug.Log($"Added: {data.segmentName}. Total: {_segments.Count}");
        }


    }

    public void ConsumeFirstSegment()
    {
        if (_segments.Count == 0)
        {
            Debug.Log("No segments to consume!");
            return;
        }

        Segment firstSegment = _segments[0];
        firstSegment.Consume();
        
        _segments.RemoveAt(0);
        Destroy(firstSegment.gameObject);

        SyncHealthToSegments();

        if (showDebugInfo)
        {
            Debug.Log($"Consumed segment. Remaining: {_segments.Count}");
        }
    }

    public void ConsumeLastSegment()
    {
        if (_segments.Count == 0)
        {
            Debug.Log("No segments to consume!");
            return;
        }

        int lastIndex = _segments.Count - 1;
        Segment lastSegment = _segments[lastIndex];
        lastSegment.Consume(); // Triggers the segment's ability
        
        if (_gameManager != null)
            _gameManager.AddScore(GetSegmentCount());

        _segments.RemoveAt(lastIndex);
        Destroy(lastSegment.gameObject);

        SyncHealthToSegments();

        if (showDebugInfo)
        {
            Debug.Log($"Consumed segment. Remaining: {_segments.Count}");
        }
    }

    
    public void RemoveLastSegment()
    {
        if (_segments.Count == 0)
        {
            // Debug.Log("No segments to remove!");
            return;
        }

        int lastIndex = _segments.Count - 1;
        Segment lastSegment = _segments[lastIndex];
        // Don't call Consume() - just remove it
        
        _segments.RemoveAt(lastIndex);
        Destroy(lastSegment.gameObject);

        SyncHealthToSegments();

        if (showDebugInfo)
        {
            Debug.Log($"Lost segment from damage. Remaining: {_segments.Count}");
        }
    }

    private void SyncHealthToSegments()
    {
        if (_healthComponent == null || !syncHealth) return;

        int segmentHealth = _segments.Count;
        _healthComponent.SetMaxHealth(segmentHealth + 1);
        _healthComponent.Heal(segmentHealth + 1); 
    }
    
    public int GetSegmentCount() => _segments.Count;
    public List<Segment> GetSegments() => new List<Segment>(_segments);

    private void OnDrawGizmos()
    {
        if (!showDebugInfo || !Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _positionHistory.Count - 1; i++)
        {
            Gizmos.DrawLine(_positionHistory[i], _positionHistory[i + 1]);
        }
    }

    public void UpdateSegmentCapacity()
    {
        maxSegments = _gameManager.GetStatData().segmentCapacity;
    }
}