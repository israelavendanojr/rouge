using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    [Header("Speaker")]
    public Speaker speaker;
    
    [Header("Content")]
    [TextArea(2, 5)] 
    public string dialogue;
    
    [Header("Appearance")]
    public Color textColor = Color.white;
    
    [Header("Timing")]
    [Tooltip("Characters per second")]
    [Range(1f, 100f)]
    public float typingSpeed = 30f;
    
    [Header("Events")]
    public UnityEvent onLineStart;
    public UnityEvent onLineComplete;
}