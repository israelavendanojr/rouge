using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSegmentInventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StatData statData; 
    [SerializeField] private GameObject segmentIconPrefab;
    
    [Header("Layout Settings")]
    [SerializeField] private bool reverseOrder = true; 
    
    private List<GameObject> spawnedIcons = new List<GameObject>();
    
    void Awake()
    {
        if (statData == null)
        {
            Debug.LogError("UpdateSegmentInventoryUI: Stat Data ScriptableObject not assigned! Please assign the StatData asset in the Inspector.");
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
        if (statData == null || segmentIconPrefab == null)
            return;
        
        // Clear existing icons
        foreach (GameObject icon in spawnedIcons)
        {
            if (icon != null)
                Destroy(icon);
        }
        spawnedIcons.Clear();
        
        List<SpawnableData> segmentsToDisplay = statData.currentSegments;
        
        if (segmentsToDisplay == null)
            return;

        List<SpawnableData> displayList = new List<SpawnableData>(segmentsToDisplay);

        if (!reverseOrder)
        {
            // Reverse the list so the last element (latest segment type added) is iterated over first
            displayList.Reverse(); 
        }

        // Create icons for each segment type
        foreach (SpawnableData segmentData in displayList)
        {
            if (segmentData == null)
                continue;
            
            GameObject iconObj = Instantiate(segmentIconPrefab, transform);
            
            Image iconImage = iconObj.GetComponent<Image>();

            if (iconImage != null && segmentData.segmentIcon != null)
            {
                iconImage.sprite = segmentData.segmentIcon;
                iconImage.color = segmentData.segmentColor; 
            }
            
            spawnedIcons.Add(iconObj);
        }
    }
}