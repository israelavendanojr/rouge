using UnityEngine;
using UnityEngine.Events; // Needed for UnityEvents

public class HealthComponent : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    
    [Header("Events")]
    public GameEvent OnDamaged;
    public GameEvent OnHealed;
    
    public GameEvent OnDeath;

    private HitFlashEffect _hitFlashEffect;

    private void Awake()
    {
        currentHealth = maxHealth;
        _hitFlashEffect = GetComponent<HitFlashEffect>();
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        
        // Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (OnDeath != null)
                OnDeath.Raise();
        }
        else
        {
            
            OnDamaged?.Raise();
        }

        _hitFlashEffect?.Flash();
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (OnHealed != null)
            OnHealed.Raise();
    }

    public void SetMaxHealth(int max)
    {
        maxHealth = max;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}