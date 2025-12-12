using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumeSegment : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;
    private SnakeSegments _snakeSegments;

    private void Awake()
    {
        _snakeSegments = GetComponent<SnakeSegments>();
    }
    private void OnEnable()
    {
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.performed += OnSegmentConsumed;
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.performed -= OnSegmentConsumed;
            interactAction.action.Disable();
        }
    }

    private void OnSegmentConsumed(InputAction.CallbackContext context)
    {
        _snakeSegments.ConsumeLastSegment();
    }

}