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

        if (onUpgrade != null)
        {
            onUpgrade.Raise();
        }
    }
}
