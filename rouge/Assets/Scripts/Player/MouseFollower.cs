using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollower : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    
    [SerializeField] InputActionReference mousePositionAction; 
    
    private Vector2 _targetWorldPosition;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    // You might also want a minimum distance to prevent jittering when close to the mouse
    [SerializeField] private float stopDistance = 0.1f;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;

        if (_rb == null)
        {
            Debug.LogError("MouseFollower requires a Rigidbody2D component on the same GameObject for physics movement.");
        }
        if (_mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure your scene has a Camera tagged 'MainCamera'.");
        }
    }

    private void OnEnable()
    {
        if (mousePositionAction != null)
        {
            mousePositionAction.action.performed += OnMousePositionPerformed;
            mousePositionAction.action.Enable(); 
        }
    }

    private void OnDisable()
    {
        if (mousePositionAction != null)
        {
            mousePositionAction.action.performed -= OnMousePositionPerformed;
            mousePositionAction.action.Disable(); 
        }
    }

    private void OnMousePositionPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 screenToWorld = new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane); 
        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(screenToWorld);
        _targetWorldPosition = new Vector2(worldPoint.x, worldPoint.y);
    }

    private void FixedUpdate()
    {
        if (_targetWorldPosition == Vector2.zero) return;

        Vector2 currentPosition = _rb.position;

        Vector2 direction = _targetWorldPosition - currentPosition;
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();

            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            Vector2 movement = direction * Mathf.Min(moveDistance, distance);
            _rb.MovePosition(currentPosition + movement);
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rb.rotation = angle - 90f; 
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
}