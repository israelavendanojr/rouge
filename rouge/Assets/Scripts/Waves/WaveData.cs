using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Wave System/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Settings")]
    public int pointBudget = 100;
    public float duration = 30f;
    public float spawnInterval = 2f;
    
    [Header("Spawnable Pool")]
    public SpawnableData[] availableSpawnables;
    
    [Header("Custom Overrides")]
    [Tooltip("If true, ignores point budget and spawns exact list below")]
    public bool useCustomSpawnables = false;
    public SpawnableData[] customSpawnables;
}