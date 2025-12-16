using UnityEngine;
using System.Collections.Generic;

public class AllyManager : MonoBehaviour
{
    [Header("Ally Settings")]
    [SerializeField] private int maxAllies = 3;
    [SerializeField] private string allyTag = "Ally";
    
    private static AllyManager _instance;
    public static AllyManager Instance => _instance;
    
    private List<GameObject> activeAllies = new List<GameObject>();
    
    private void Awake()
    {
        // Singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }
    
    private void Start()
    {
        // Find all existing allies in the scene
        UpdateAllyList();
    }
    
    public void RegisterAlly(GameObject ally)
    {
        if (!activeAllies.Contains(ally))
        {
            activeAllies.Add(ally);
        }
    }
    
    public void UnregisterAlly(GameObject ally)
    {
        activeAllies.Remove(ally);
    }
    
    public bool CanSpawnAlly()
    {
        // Clean up destroyed allies
        activeAllies.RemoveAll(ally => ally == null);
        
        return activeAllies.Count < maxAllies;
    }
    
    public int GetActiveAllyCount()
    {
        activeAllies.RemoveAll(ally => ally == null);
        return activeAllies.Count;
    }
    
    public int GetMaxAllies() => maxAllies;
    
    private void UpdateAllyList()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag(allyTag);
        activeAllies.Clear();
        
        foreach (GameObject ally in allies)
        {
            activeAllies.Add(ally);
        }
    }
    
    // Optional: Public method to change max allies at runtime
    public void SetMaxAllies(int newMax)
    {
        maxAllies = Mathf.Max(1, newMax);
    }
}