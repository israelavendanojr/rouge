using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnable", menuName = "Wave System/Spawnable Data")]
public class SpawnableData : ScriptableObject
{
    public GameObject prefab;
    public int cost = 1;
    [Range(0f, 1f)] public float spawnWeight = 1f;
}