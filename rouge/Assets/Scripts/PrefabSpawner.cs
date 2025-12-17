using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool parentToThis = false;

    /// <summary>
    /// Spawns the prefab.
    /// </summary>
    public void Spawn()
    {
        if (prefab == null)
        {
            Debug.LogWarning($"{name}: No prefab assigned!");
            return;
        }

        Transform parent = parentToThis ? transform : null;
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;

        Instantiate(prefab, position, Quaternion.identity, parent);
    }

    /// <summary>
    /// Spawns the prefab at a specific position.
    /// </summary>
    public void SpawnAt(Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }
}
