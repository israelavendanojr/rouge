using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Speaker")]
public class Speaker : ScriptableObject
{
    [Header("Identity")]
    public string speakerName;
    public Sprite portrait;
    
    [Header("Audio")]
    public SimpleAudioEvent voiceSound;
    
}