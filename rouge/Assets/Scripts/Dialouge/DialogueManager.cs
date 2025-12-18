using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Diagnostics;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image speakerPortrait;
    
    [Header("Conversation")]
    [SerializeField] private Conversation conversation;
    
    [Header("Input")]
    // [SerializeField] private InputActionReference advanceAction;
    
    [Header("Settings")]
    [SerializeField] private bool autoStartOnEnable = true;
    [SerializeField] private bool canSkipTyping = true;
    
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool conversationActive = false;
    private Coroutine typingCoroutine;
    
    private void OnEnable()
    {
        // if (advanceAction != null)
        // {
        //     advanceAction.action.performed += OnAdvancePressed;
        //     advanceAction.action.Enable();
        // }
        
        if (autoStartOnEnable && conversation != null)
        {
            StartConversation();
        }
    }
    
    private void OnDisable()
    {
        // if (advanceAction != null)
        // {
        //     advanceAction.action.performed -= OnAdvancePressed;
        //     advanceAction.action.Disable();
        // }
    }
    
    public void StartConversation()
    {
        if (conversation == null || conversation.dialogueLines.Length == 0)
        {
            UnityEngine.Debug.LogWarning("No conversation assigned or conversation is empty!");
            return;
        }
        
        conversationActive = true;
        currentLineIndex = 0;
        
        conversation.onConversationStart?.Invoke();
        
        DisplayLine(currentLineIndex);
    }
    
    public void SetConversation(Conversation newConversation)
    {
        conversation = newConversation;
    }
    
    public void OnAdvancePressed()
    {

        UnityEngine.Debug.Log("Advance pressed");
        if (!conversationActive) return;
        
        if (isTyping && canSkipTyping)
        {
            CompleteCurrentLine();
        }
        else if (!isTyping)
        {
            AdvanceConversation();
        }
    }
    
    private void DisplayLine(int lineIndex)
    {
        if (lineIndex >= conversation.dialogueLines.Length)
        {
            EndConversation();
            return;
        }
        
        DialogueLine line = conversation.dialogueLines[lineIndex];
        
        // Update speaker info
        if (line.speaker != null)
        {
            if (line.speaker.speakerName != null)
            {
                speakerNameText.text = line.speaker.speakerName;                
            }
            
            if (speakerPortrait != null && line.speaker.portrait != null)
            {
                speakerPortrait.sprite = line.speaker.portrait;
                speakerPortrait.enabled = true;
            }
            else if (speakerPortrait != null)
            {
                speakerPortrait.enabled = false;
            }
        }
        else
        {
            speakerNameText.text = "";
            if (speakerPortrait != null)
                speakerPortrait.enabled = false;
        }

        
        
        // Invoke line start event
        line.onLineStart?.Invoke();
        
        // Start typing animation
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine(line));
    }
    
    private IEnumerator TypeLine(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";
        dialogueText.color = line.textColor;
        
        float delayPerCharacter = 1f / line.typingSpeed;
        
        foreach (char c in line.dialogue)
        {
            dialogueText.text += c;
            
            // Play voice sound for non-whitespace characters
            if (!char.IsWhiteSpace(c) && line.speaker != null && line.speaker.voiceSound != null)
            {
                line.speaker.voiceSound.Play();
            }
            
            yield return new WaitForSeconds(delayPerCharacter);
        }
        
        isTyping = false;
        line.onLineComplete?.Invoke();
    }
    
    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        DialogueLine line = conversation.dialogueLines[currentLineIndex];
        dialogueText.text = line.dialogue;
        dialogueText.color = line.textColor;
        isTyping = false;
        
        line.onLineComplete?.Invoke();
    }
    
    private void AdvanceConversation()
    {
        dialogueText.alpha = 255;

        currentLineIndex++;
        
        if (currentLineIndex < conversation.dialogueLines.Length)
        {
            DisplayLine(currentLineIndex);
        }
        else
        {
            EndConversation();
        }
    }
    
    private void EndConversation()
    {
        conversationActive = false;
        conversation.onConversationEnd?.Invoke();
        
        // Clear UI
        speakerNameText.text = "";
        dialogueText.text = "";
        
        if (speakerPortrait != null)
            speakerPortrait.enabled = false;
    }
    
    // Public methods for manual control
    public void Next()
    {
        if (isTyping && canSkipTyping)
        {
            CompleteCurrentLine();
        }
        else if (!isTyping)
        {
            AdvanceConversation();
        }
    }
    
    public bool IsActive() => conversationActive;
    public bool IsTyping() => isTyping;
}