using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UpdateValueUI : MonoBehaviour
{
    [Header("Value Source")]
    private GameManager gameManager;
    
    [Header("Value Type")]
    [SerializeField] private ValueType valueType;
    
    [Header("Formatting")]
    [SerializeField] private string prefix = "";
    [SerializeField] private string suffix = "";
    
    private TextMeshProUGUI textComponent;
    
    public enum ValueType
    {
        Wave,
        Score
    }
    
    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            
            if (gameManager == null)
            {
                Debug.LogError("UpdateValueUI: GameManager not found!");
            }
        }
    }
    
    void Start()
    {
        // Initial update
        UpdateDisplay();
    }
    
    public void UpdateDisplay()
    {
        if (gameManager == null || textComponent == null)
            return;
        
        int value = valueType == ValueType.Wave ? gameManager.GetWave() : gameManager.GetScore();
        textComponent.text = $"{prefix}{value}{suffix}";
    }
}