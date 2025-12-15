using UnityEngine;

public class SegmentAnimationController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the StatData ScriptableObject")]
    public StatData statData;

    private Animator[] childAnimators;

    private void Awake()
    {
        // Get all child animators
        childAnimators = new Animator[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childAnimators[i] = transform.GetChild(i).GetComponent<Animator>();
            
            if (childAnimators[i] == null)
            {
                Debug.LogWarning($"Child {i} ({transform.GetChild(i).name}) does not have an Animator component!");
            }
        }

        if (statData == null)
        {
            Debug.LogError("StatData reference is missing!");
            return;
        }

        // Ensure segmentTypes array matches child count
        if (statData.segmentTypes.Count != childAnimators.Length)
        {
            Debug.LogWarning($"StatData.segmentTypes count ({statData.segmentTypes.Count}) does not match child count ({childAnimators.Length})!");
        }
    }

    private void Start()
    {
        UpdateAnimations();
    }

    public void UpdateAnimations()
    {
        if (statData == null || statData.currentSegments == null || statData.segmentTypes == null || childAnimators == null) return;

        for (int i = 0; i < childAnimators.Length && i < statData.segmentTypes.Count; i++)
        {
            if (childAnimators[i] == null) continue;

            if (statData.currentSegments.Contains(statData.segmentTypes[i]))
            {
                // Play animation
                childAnimators[i].speed = 1f;
            }
            else
            {
                // Pause animation
                childAnimators[i].speed = 0f;
            }
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying && statData != null)
        {
            UpdateAnimations();
        }
    }
}