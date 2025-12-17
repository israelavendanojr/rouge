using UnityEngine;

/// <summary>
/// Flips a sprite's X scale based on movement direction.
/// Attach to any GameObject with a sprite that needs to face its movement direction.
/// </summary>
public class FlipSpriteToDirection : MonoBehaviour
{
    [Header("Flip Settings")]
    [Tooltip("Which direction is considered 'right' in your sprite")]
    [SerializeField] private bool spriteDefaultFacesRight = true;
    
    [Tooltip("Minimum velocity to trigger a flip (prevents jittering)")]
    [SerializeField] private float velocityThreshold = 0.1f;
    
    [Header("Optional: Manual Target")]
    [Tooltip("If set, faces toward this target instead of using velocity")]
    [SerializeField] private Transform targetToFace;
    
    private Rigidbody2D rb;
    private bool currentlyFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentlyFacingRight = spriteDefaultFacesRight;
    }

    private void LateUpdate()
    {
        if (targetToFace != null)
        {
            FlipTowardTarget();
        }
        else if (rb != null)
        {
            FlipBasedOnVelocity();
        }
    }

    private void FlipBasedOnVelocity()
    {
        // Only flip if moving fast enough
        if (Mathf.Abs(rb.velocity.x) < velocityThreshold)
            return;

        bool shouldFaceRight = rb.velocity.x > 0;
        
        if (shouldFaceRight != currentlyFacingRight)
        {
            Flip();
        }
    }

    private void FlipTowardTarget()
    {
        float direction = targetToFace.position.x - transform.position.x;
        
        // Only flip if direction is significant
        if (Mathf.Abs(direction) < velocityThreshold)
            return;

        bool shouldFaceRight = direction > 0;
        
        if (shouldFaceRight != currentlyFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        currentlyFacingRight = !currentlyFacingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    /// <summary>
    /// Manually set the facing direction
    /// </summary>
    public void SetFacingRight(bool faceRight)
    {
        if (faceRight != currentlyFacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Get current facing direction
    /// </summary>
    public bool IsFacingRight() => currentlyFacingRight;
}