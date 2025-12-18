using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ButtonPressHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference buttonAction;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onButtonPressed;
    
    private void OnEnable()
    {
        if (buttonAction != null)
        {
            buttonAction.action.Enable();
        }
    }
    
    private void OnDisable()
    {
        if (buttonAction != null)
        {
            buttonAction.action.Disable();
        }
    }
    
    private void Update()
    {
        if (buttonAction != null && buttonAction.action.WasPressedThisFrame())
        {
            onButtonPressed?.Invoke();
        }
    }
}