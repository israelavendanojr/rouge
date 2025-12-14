using UnityEngine;

[CreateAssetMenu(fileName = "NewSegmentData", menuName = "Snake/Segment Data")]
public class SegmentData : ScriptableObject
{
    [Header("Visual")]
    public Color segmentColor = Color.white;
    public Sprite segmentSprite;
    public Sprite segmentIcon;
    
    [Header("Properties")]
    public string segmentName;
    [TextArea(2, 4)]
    public string description;

    public virtual void OnConsume(Vector3 position, Quaternion rotation)
    {
        // Base implementation - override in derived classes
    }
}
    
    