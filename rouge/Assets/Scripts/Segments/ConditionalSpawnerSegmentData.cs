using UnityEngine;

[CreateAssetMenu(fileName = "NewConditionalSpawner", menuName = "Snake/Conditional Spawner Segment")]
public class ConditionalSpawnerSegmentData : SegmentData
{
    [Header("Primary Spawn")]
    public GameObject[] primaryPrefabs;
    public string checkTag = "Ally";
    
    public GameObject[] fallbackPrefabs;
    
    public override void OnConsume(Vector3 position, Quaternion rotation, int level)
    {
        
        GameObject prefabToSpawn = null;
        
        if (primaryPrefabs != null && primaryPrefabs.Length > 0)
        {
                int index = Mathf.Clamp(level - 1, 0, primaryPrefabs.Length - 1);
                prefabToSpawn = primaryPrefabs[index];
        }
        else
        {
            // Spawn fallback
            if (fallbackPrefabs != null && fallbackPrefabs.Length > 0)
            {
                int index = Mathf.Clamp(level - 1, 0, fallbackPrefabs.Length - 1);
                prefabToSpawn = fallbackPrefabs[index];
            }
        }
        
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"No prefab assigned for {segmentName} at level {level}");
        }
    }
}