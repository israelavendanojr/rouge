using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSegmentInventoryUI : MonoBehaviour
{
    [Header("References")]
    private SnakeSegments snakeSegments;
    [SerializeField] private GameObject segmentIconPrefab;
    
    [Header("Layout Settings")]
    [SerializeField] private bool reverseOrder = false; // True = bottom-to-top display
    
    private List<GameObject> spawnedIcons = new List<GameObject>();
    
    void Awake()
    {
        if (snakeSegments == null)
        {
            snakeSegments = FindObjectOfType<SnakeSegments>();
            
            if (snakeSegments == null)
            {
                Debug.LogError("UpdateSegmentInventoryUI: SnakeSegments not found!");
            }
        }
        
        if (segmentIconPrefab == null)
        {
            Debug.LogError("UpdateSegmentInventoryUI: Segment Icon Prefab not assigned!");
        }
    }
    
    void Start()
    {
        UpdateDisplay();
    }
    
    public void UpdateDisplay()
    {
        if (snakeSegments == null || segmentIconPrefab == null)
            return;
        
        // Clear existing icons
        foreach (GameObject icon in spawnedIcons)
        {
            if (icon != null)
                Destroy(icon);
        }
        spawnedIcons.Clear();
        
        // Get current segments
        List<Segment> segments = snakeSegments.GetSegments();
        
        // Create icons for each segment
        List<Segment> displaySegments = reverseOrder ? segments : new List<Segment>(segments);
        if (!reverseOrder)
            displaySegments.Reverse(); // Show last-added at top
        
        foreach (Segment segment in displaySegments)
        {
            if (segment == null || segment.Data == null)
                continue;
            
            GameObject iconObj = Instantiate(segmentIconPrefab, transform);
            
            // Set the icon sprite
            Image iconImage = iconObj.GetComponent<Image>();
            if (iconImage != null && segment.Data.segmentIcon != null)
            {
                iconImage.sprite = segment.Data.segmentIcon;
                iconImage.color = segment.Data.segmentColor;
            }
            
            spawnedIcons.Add(iconObj);
        }
    }
}