using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SimpleTextUpdater : MonoBehaviour
{
    [Header("Optional Formatting")]
    [SerializeField] private string prefix = "";
    [SerializeField] private string suffix = "";

    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError($"{name}: No TextMeshProUGUI found!");
        }
    }

    /// <summary>
    /// Updates the text box with the given value.
    /// </summary>
    public void UpdateText(string newText)
    {
        if (textComponent != null)
        {
            textComponent.text = $"{prefix}{newText}{suffix}";
        }
    }
}
