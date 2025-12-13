using UnityEngine;
using UnityEngine.Events; // Needed for UnityEvents

public class HealthComponent : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    
    public GameEvent OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (OnDeath != null)
                OnDeath.Raise();
        }
    }
    
    public void RestoreHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}