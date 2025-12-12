using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class SegmentPickup : MonoBehaviour
{
    [SerializeField] private SegmentData segmentData;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        
        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        if (segmentData == null || _spriteRenderer == null) return;
        
        _spriteRenderer.color = segmentData.segmentColor;
        
        if (segmentData.segmentSprite != null)
        {
            _spriteRenderer.sprite = segmentData.segmentSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SnakeSegments snake = other.GetComponent<SnakeSegments>();
        
        if (snake != null)
        {
            snake.AddSegment(segmentData);
            Destroy(gameObject); 
        }
    }

    private void OnValidate()
    {
        if (segmentData != null && _spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        ApplyVisuals();
    }
}