using UnityEngine;

public class PickRightColor : MonoBehaviour
{
    public enum OrnamentColor
    {
        Red,
        Green,
        Blue
    }

    [Header("Settings")]
    public OrnamentColor selectedColor = OrnamentColor.Red;

    private Animator animator;
    private OrnamentColor previousColor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    private void Start()
    {
        PlayColorAnimation();
        previousColor = selectedColor;
    }

    private void Update()
    {
        // Check if color changed in inspector
        if (previousColor != selectedColor)
        {
            PlayColorAnimation();
            previousColor = selectedColor;
        }
    }

    public void SetColor(OrnamentColor color)
    {
        selectedColor = color;
        PlayColorAnimation();
    }

    private void PlayColorAnimation()
    {
        if (animator == null) return;

        switch (selectedColor)
        {
            case OrnamentColor.Red:
                animator.Play("Red");
                break;
            case OrnamentColor.Green:
                animator.Play("Green");
                break;
            case OrnamentColor.Blue:
                animator.Play("Blue");
                break;
        }
    }
}