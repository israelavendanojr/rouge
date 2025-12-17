using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStat", menuName = "Wave System/Stat Data")]
public class StatData : ScriptableObject
{
    public List<SpawnableData> currentSegments;
    public int GetSegmentLength() => currentSegments.Count;
    
    public List<SpawnableData> segmentTypes;
    public int level = 1;
    public int GetLevel() => level;
    public int segmentCapacity = 1;
    public int GetSegmentCapacity() => segmentCapacity;
    public int waveNumber = 0;
    public int score = 0;

    public void InitializeStats()
    {
        currentSegments.Clear();
        currentSegments.Add(GetNewSegmentType());
        
        level = 1;
        segmentCapacity = 1;
        waveNumber = 0;
        score = 0;
    }

    public SpawnableData GetNewSegmentType()
    {
        if (segmentTypes == null || segmentTypes.Count == 0)
        {
            Debug.LogWarning("SegmentTypes is empty.");
            return null;
        }

        // If all segment types are already used, stop rerolling
        if (currentSegments != null && currentSegments.Count >= segmentTypes.Count)
        {
            Debug.LogWarning("All segment types are already in use.");
            return null;
        }

        SpawnableData selected;
        do
        {
            selected = segmentTypes[Random.Range(0, segmentTypes.Count)];
        }
        while (currentSegments != null && currentSegments.Contains(selected));

        return selected;
    }

    public void IncreaseLevel(int amount)
    {
        level += amount;
    }

    public void IncreaseSegmentCapacity(int amount)
    {
        segmentCapacity += amount;
    }

    public void AddSegmentType(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnableData newSegment = GetNewSegmentType();
            currentSegments.Add(newSegment);
        }
    }
}