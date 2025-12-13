using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Configuration")]
    public WaveData[] waves;
    public int currentWaveIndex = 0;
    
    [Header("Spawn Settings")]
    public Transform[] spawnLocations;
    
    [Header("Runtime Data")]
    private List<GameObject> itemsToSpawn = new List<GameObject>();
    private List<GameObject> spawnedItems = new List<GameObject>();
    private int spawnLocationIndex = 0;
    
    private float waveTimer;
    private float spawnInterval; // Keep this field
    private float spawnTimer;
    
    void Start()
    {
        if (waves.Length > 0)
            StartWave();
    }
    
    void FixedUpdate()
    {
        // Check if there are items to spawn and the spawn timer is ready
        if (spawnTimer <= 0 && itemsToSpawn.Count > 0)
        {
            SpawnNextItem();
            // Reset the timer using the interval from WaveData
            spawnTimer = spawnInterval;
        }
        else
        {
            // Decrement the timers
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
        
        // Clean up destroyed items
        spawnedItems.RemoveAll(item => item == null);
        
        // Check if wave is complete
        if (waveTimer <= 0 && spawnedItems.Count <= 0 && itemsToSpawn.Count <= 0)
        {
            currentWaveIndex++;
            if (currentWaveIndex < waves.Length)
                StartWave();
        }
    }
    
    void StartWave()
    {
        WaveData wave = waves[currentWaveIndex];
        
        if (wave.useCustomSpawnables)
        {
            GenerateCustomItems(wave);
        }
        else
        {
            GenerateItems(wave);
        }
        
        // --- CHANGE 1: Use the interval from WaveData directly ---
        spawnInterval = wave.spawnInterval;
        // --------------------------------------------------------

        waveTimer = wave.duration;
        spawnTimer = 0; // Spawn first item immediately
    }
    
    void GenerateItems(WaveData wave)
    {
        itemsToSpawn.Clear();
        int remainingPoints = wave.pointBudget;
        int maxItems = 50;
        
        while (remainingPoints > 0 && itemsToSpawn.Count < maxItems)
        {
            SpawnableData spawnable = GetRandomItem(wave.availableSpawnables);
            
            if (spawnable != null && remainingPoints >= spawnable.cost)
            {
                itemsToSpawn.Add(spawnable.prefab);
                remainingPoints -= spawnable.cost;
            }
            else
            {
                break; // No affordable items left
            }
        }
    }
    
    void GenerateCustomItems(WaveData wave)
    {
        itemsToSpawn.Clear();
        foreach (SpawnableData spawnable in wave.customSpawnables)
        {
            if (spawnable != null && spawnable.prefab != null)
                itemsToSpawn.Add(spawnable.prefab);
        }
    }
    
    SpawnableData GetRandomItem(SpawnableData[] spawnables)
    {
        if (spawnables.Length == 0) return null;
        
        // Simple random selection (can be upgraded to weighted selection)
        return spawnables[Random.Range(0, spawnables.Length)];
    }
    
    void SpawnNextItem()
    {
        if (itemsToSpawn.Count == 0 || spawnLocations.Length == 0) return;
        
        GameObject itemPrefab = itemsToSpawn[0];
        itemsToSpawn.RemoveAt(0);
        
        Transform spawnPoint = spawnLocations[spawnLocationIndex];
        GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        spawnedItems.Add(spawnedItem);
        
        spawnLocationIndex = (spawnLocationIndex + 1) % spawnLocations.Length;
    }
}