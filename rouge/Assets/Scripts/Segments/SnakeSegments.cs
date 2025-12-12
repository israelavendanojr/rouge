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
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    private void Start()
    {
        _positionHistory.Add(transform.position);
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
    }

    public void AddSegment(SegmentData data)
    {
        // 1. Check if max capacity is reached
        if (_segments.Count >= maxSegments)
        {
            // If at max, remove the oldest segment (the first one) to make room
            if (showDebugInfo)
            {
                Debug.Log($"Max segments ({maxSegments}) reached. Consuming oldest segment to make room.");
            }
            // Use ConsumeFirstSegment to free up a slot
            ConsumeFirstSegment();
        }

        // 2. Continue with the adding process (which is now guaranteed to not exceed maxSegments)

        if (segmentPrefab == null)
        {
            Debug.LogError("Segment prefab is not assigned in SnakeSegments!");
            return;
        }

        if (data == null)
        {
            Debug.LogError("Cannot add segment with null data!");
            return;
        }

        Vector3 spawnPosition;
        if (_segments.Count > 0)
        {
            // Spawn at the position of the new last segment (which was the old last segment's spot)
            spawnPosition = _segments[_segments.Count - 1].transform.position;
        }
        else
        {
            // Only runs if the snake was initially empty
            spawnPosition = transform.position - transform.up * segmentSpacing;
        }

        GameObject segmentObj = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
        segmentObj.transform.parent = transform;
        
        Segment segment = segmentObj.GetComponent<Segment>();
        if (segment == null)
        {
            Debug.LogError("Segment prefab doesn't have a Segment component!");
            Destroy(segmentObj);
            return;
        }

        segment.Initialize(data);
        
        _segments.Add(segment);

        if (showDebugInfo)
        {
            Debug.Log($"Added segment: {data.segmentName}. Total segments: {_segments.Count}");
        }
    }

    public void ConsumeFirstSegment()
    {
        if (_segments.Count == 0)
        {
            Debug.LogWarning("No segments to consume!");
            return;
        }

        Segment firstSegment = _segments[0];
        firstSegment.Consume();
        
        _segments.RemoveAt(0);
        Destroy(firstSegment.gameObject);

        if (showDebugInfo)
        {
            Debug.Log($"Consumed segment. Remaining segments: {_segments.Count}");
        }
    }

    public void ConsumeLastSegment()
    {
        if (_segments.Count == 0)
        {
            Debug.LogWarning("No segments to consume!");
            return;
        }

        int lastIndex = _segments.Count - 1;
        Segment lastSegment = _segments[lastIndex];
        lastSegment.Consume();
        
        _segments.RemoveAt(lastIndex);
        Destroy(lastSegment.gameObject);

        if (showDebugInfo)
        {
            Debug.Log($"Consumed segment. Remaining segments: {_segments.Count}");
        }
    }
    
    public int GetSegmentCount()
    {
        return _segments.Count;
    }

    public List<Segment> GetSegments()
    {
        return new List<Segment>(_segments);
    }

    private void OnDrawGizmos()
    {
        if (!showDebugInfo || !Application.isPlaying) return;
        
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _positionHistory.Count - 1; i++)
        {
            Gizmos.DrawLine(_positionHistory[i], _positionHistory[i + 1]);
        }
    }
}