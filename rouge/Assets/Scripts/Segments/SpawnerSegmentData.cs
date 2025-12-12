using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnerSegment", menuName = "Snake/Spawner Segment Data")]
public class SpawnerSegmentData : SegmentData
{
    [Header("Spawner Settings")]
    public GameObject spawnPrefab;
    
    public override void OnConsume(Vector3 position, Quaternion rotation)
    {
        if (spawnPrefab != null)
        {
            Instantiate(spawnPrefab, position, rotation);
        }
        else
        {
            Debug.LogWarning($"No spawn prefab assigned for {segmentName}");
        }
    }
}