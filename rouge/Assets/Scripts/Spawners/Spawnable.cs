using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawnable : MonoBehaviour
{
    [Header("Spawn Pool")]
    [SerializeField] private List<SpawnableData> spawnableItems;
    
    [Header("Spawn Budget")]
    public int budget = 50;
    
    [Header("Spawn Behavior")]
    public bool spawnAtThisPosition = true;
    public Transform customSpawnPoint;
    
    private int remainingBudget;
    
    void Awake()
    {
        remainingBudget = budget;
        
        StatData statData = FindObjectOfType<GameManager>().GetStatData();
        spawnableItems = statData.currentSegments;

    }
    
    public void SpawnAll()
    {
        while (remainingBudget > 0 && spawnableItems.Count > 0)
        {
            SpawnableData item = GetWeightedRandomItem();
            
            if (item != null && remainingBudget >= item.cost)
            {
                SpawnItem(item);
                remainingBudget -= item.cost;
            }
            else
            {
                break;
            }
        }
    }
    
    public bool SpawnOne()
    {
        if (remainingBudget <= 0 || spawnableItems.Count == 0)
            return false;
        
        SpawnableData item = GetWeightedRandomItem();
        
        if (item != null && remainingBudget >= item.cost)
        {
            SpawnItem(item);
            remainingBudget -= item.cost;
            return true;
        }
        
        return false;
    }
    
    public bool SpawnByIndex(int index)
    {
        if (index < 0 || index >= spawnableItems.Count)
            return false;
        
        SpawnableData item = spawnableItems[index];
        
        if (item != null && remainingBudget >= item.cost)
        {
            SpawnItem(item);
            remainingBudget -= item.cost;
            return true;
        }
        
        return false;
    }
    
    public void ResetBudget()
    {
        remainingBudget = budget;
    }
    
    public void AddBudget(int amount)
    {
        remainingBudget += amount;
    }
    
    public int GetRemainingBudget() => remainingBudget;
    
    void SpawnItem(SpawnableData spawnableData)
    {
        Vector3 spawnPosition = spawnAtThisPosition ? transform.position : customSpawnPoint.position;
        Quaternion spawnRotation = spawnAtThisPosition ? transform.rotation : customSpawnPoint.rotation;
        
        Instantiate(spawnableData.prefab, spawnPosition, spawnRotation);
    }
    
    SpawnableData GetWeightedRandomItem()
    {
        if (spawnableItems.Count == 0) return null;
        
        float totalWeight = 0f;
        foreach (SpawnableData item in spawnableItems)
        {
            totalWeight += item.spawnWeight;
        }
        
        float randomPoint = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        foreach (SpawnableData item in spawnableItems)
        {
            currentWeight += item.spawnWeight;
            if (randomPoint <= currentWeight)
                return item;
        }
        
        return spawnableItems[0];
    }
}