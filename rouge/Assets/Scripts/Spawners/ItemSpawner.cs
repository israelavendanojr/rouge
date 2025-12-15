using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public SpawnableData[] spawnableItems;
    
    [Header("Spawn Locations")]
    public Transform[] spawnLocations;
    
    [Header("Spawn Timing")]
    public float spawnInterval = 2f;
    
    private HealthComponent healthComponent;
    private float spawnTimer;
    
    void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        spawnableItems = FindObjectOfType<GameManager>().GetStatData().currentSegments.ToArray();
    }
    
    void Start()
    {
        spawnTimer = spawnInterval;
    }
    
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        
        if (spawnTimer <= 0)
        {
            TrySpawnItem();
            spawnTimer = spawnInterval;
        }
    }
    
    void TrySpawnItem()
    {
        if (healthComponent.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        if (spawnableItems.Length == 0 || spawnLocations.Length == 0)
            return;
        
        SpawnableData selectedItem = GetWeightedRandomItem();
        
        if (selectedItem != null && healthComponent.GetCurrentHealth() >= selectedItem.cost)
        {
            SpawnItem(selectedItem);
            healthComponent.TakeDamage(selectedItem.cost);
        }
    }
    
    void SpawnItem(SpawnableData spawnableData)
    {
        Transform spawnPoint = spawnLocations[Random.Range(0, spawnLocations.Length)];
        Instantiate(spawnableData.prefab, spawnPoint.position, Quaternion.identity);
    }
    
    SpawnableData GetWeightedRandomItem()
    {
        // Calculate total weight
        float totalWeight = 0f;
        foreach (SpawnableData item in spawnableItems)
        {
            totalWeight += item.spawnWeight;
        }
        
        // Select random point in weight range
        float randomPoint = Random.Range(0f, totalWeight);
        
        // Find item at that point
        float currentWeight = 0f;
        foreach (SpawnableData item in spawnableItems)
        {
            currentWeight += item.spawnWeight;
            if (randomPoint <= currentWeight)
                return item;
        }
        
        // Fallback to first item
        return spawnableItems[0];
    }
}