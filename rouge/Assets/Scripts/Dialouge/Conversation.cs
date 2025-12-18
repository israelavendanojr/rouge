using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject
{
    [Header("Dialogue")]
    public DialogueLine[] dialogueLines;
    
    [Header("Events")]
    public UnityEvent onConversationStart;
    public UnityEvent onConversationEnd;
}