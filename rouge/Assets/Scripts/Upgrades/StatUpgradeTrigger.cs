using UnityEngine;

public class StatUpgradeTrigger : MonoBehaviour
{
    public StatData statData;
    [SerializeField] GameObject spawnEffect;

    public enum UpgradeType
    {
        IncreaseLevel,
        SpawnItem,
        AddSegmentType
    }

    public UpgradeType upgradeType;
    public int amount = 1; 
    [SerializeField] GameEvent onUpgrade;
    [SerializeField] GameEvent upgradeSpecificEvent;

    void Start()
    {
        switch (upgradeType)
        {
            case UpgradeType.IncreaseLevel:
                if (statData.maxLevel <= statData.GetLevel())
                {
                    Destroy(gameObject, 0f);
                }
                break;

            case UpgradeType.SpawnItem:
                GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
                if (statData.maxAllies <= allies.Length)
                {
                    Destroy(gameObject, 0f);
                }
                break;

            case UpgradeType.AddSegmentType:
                if (statData.segmentTypes.Count <= statData.currentSegments.Count)
                {
                    Destroy(gameObject, 0f);
                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        ApplyUpgrade();

        Destroy(gameObject, 0f);
    }

    private void ApplyUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.IncreaseLevel:
                statData.IncreaseLevel(amount);
                statData.IncreaseSegmentCapacity(amount); 
                break;

            case UpgradeType.SpawnItem:
                Instantiate(spawnEffect, transform.position, Quaternion.identity);
                break;

            case UpgradeType.AddSegmentType:
                statData.AddSegmentType(amount);
                break;
        }

        onUpgrade?.Raise();
        upgradeSpecificEvent?.Raise();
    }
}
