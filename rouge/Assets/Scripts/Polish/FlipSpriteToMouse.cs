using UnityEngine;

/// <summary>
/// Flips a sprite based on mouse position or movement direction.
/// Perfect for player characters and cursor-following objects.
/// </summary>
public class FlipSpriteToMouse : MonoBehaviour
{
    [Header("Flip Mode")]
    [Tooltip("Mouse: Flip based on mouse position relative to sprite\nMovement: Flip based on actual movement direction")]
    [SerializeField] private FlipMode flipMode = FlipMode.Mouse;
    
    [Header("Flip Settings")]
    [Tooltip("Which direction is considered 'right' in your sprite")]
    [SerializeField] private bool spriteDefaultFacesRight = true;
    
    [Tooltip("Minimum distance/velocity to trigger a flip (prevents jittering)")]
    [SerializeField] private float threshold = 0.1f;
    
    private Camera mainCamera;
    private Rigidbody2D rb;
    private bool currentlyFacingRight;
    private Vector3 lastPosition;

    public enum FlipMode
    {
        Mouse,      // Flip based on mouse position
        Movement    // Flip based on actual movement
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        currentlyFacingRight = spriteDefaultFacesRight;
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        switch (flipMode)
        {
            case FlipMode.Mouse:
                FlipTowardMouse();
                break;
            case FlipMode.Movement:
                FlipBasedOnMovement();
                break;
        }
    }

    private void FlipTowardMouse()
    {
        if (mainCamera == null)
            return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float direction = mouseWorldPos.x - transform.position.x;
        
        // Only flip if mouse is far enough away
        if (Mathf.Abs(direction) < threshold)
            return;

        bool shouldFaceRight = direction > 0;
        
        if (shouldFaceRight != currentlyFacingRight)
        {
            Flip();
        }
    }

    private void FlipBasedOnMovement()
    {
        Vector3 movement = transform.position - lastPosition;
        
        // Only flip if moving fast enough
        if (Mathf.Abs(movement.x) < threshold * Time.deltaTime)
        {
            lastPosition = transform.position;
            return;
        }

        bool shouldFaceRight = movement.x > 0;
        
        if (shouldFaceRight != currentlyFacingRight)
        {
            Flip();
        }
        
        lastPosition = transform.position;
    }

    private void Flip()
    {
        currentlyFacingRight = !currentlyFacingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    /// <summary>
    /// Change flip mode at runtime
    /// </summary>
    public void SetFlipMode(FlipMode mode)
    {
        flipMode = mode;
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