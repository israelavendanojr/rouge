using UnityEngine;

public class StatUpgradeTrigger : MonoBehaviour
{
    public StatData statData;

    public enum UpgradeType
    {
        IncreaseLevel,
        IncreaseSegmentCapacity,
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

        Destroy(gameObject);
    }

    private void ApplyUpgrade()
    {
        switch (upgradeType)
        {
            case UpgradeType.IncreaseLevel:
                statData.IncreaseLevel(amount);
                break;

            case UpgradeType.IncreaseSegmentCapacity:
                statData.IncreaseSegmentCapacity(amount);
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
