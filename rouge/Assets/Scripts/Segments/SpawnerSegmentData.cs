using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnerSegment", menuName = "Snake/Spawner Segment Data")]
public class SpawnerSegmentData : SegmentData
{
    [Header("Spawner Settings")]
    public GameObject[] spawnPrefabs;
    
    public override void OnConsume(Vector3 position, Quaternion rotation, int level)
    {
        GameObject prefab = spawnPrefabs[level - 1];
        Quaternion uprightRotation = Quaternion.identity;
        if (prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"No spawn prefab assigned for {segmentName}");
        }
    }
}