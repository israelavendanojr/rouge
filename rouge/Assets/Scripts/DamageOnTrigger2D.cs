using UnityEngine;

public class DamageOnTrigger2D : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private string targetTag = "Target";

    private HealthComponent health;

    void awake()
    {
        health = GetComponent<HealthComponent>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
            return;

        
        health?.TakeDamage(damage);
        if (health != null && health.GetCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
