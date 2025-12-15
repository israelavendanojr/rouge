using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Segment : MonoBehaviour
{
    [SerializeField] private SegmentData _data;
    private SpriteRenderer _spriteRenderer;
    
    public SegmentData Data => _data;
    private GameManager _gameManager;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void Initialize(SegmentData data)
    {
        _data = data;
        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        if (_data == null || _spriteRenderer == null) return;
        
        _spriteRenderer.color = _data.segmentColor;
        
        if (_data.segmentSprite != null)
        {
            _spriteRenderer.sprite = _data.segmentSprite;
        }
    }

    public void Consume()
    {
        // Debug.Log($"Consumed segment: {_data.segmentName}");
        _data.OnConsume(transform.position, transform.rotation, _gameManager.GetStatData().level);
    }

    private void OnValidate()
    {
        // Update visuals in editor when data changes
        if (_data != null && _spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        ApplyVisuals();
    }
}