using UnityEngine;

public abstract class BaseEnemy : StateMachine
{
    [Header("Components")]
    [HideInInspector] public HealthComponent healthComponent;
    protected Rigidbody2D rb;
    public Rigidbody2D GetRigidbody() => rb;
    protected Spawnable spawnable;
    public Spawnable GetSpawnable() => spawnable;

    [Header("States")]
    protected EnemyIdleState idleState;
    protected EnemyDeathState deathState;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponent>();
        spawnable = GetComponent<Spawnable>();
        
        if (healthComponent == null)
        {
            Debug.LogError("BaseEnemy requires a HealthComponent on the same GameObject!");
        }

        idleState = new EnemyIdleState(this);
        deathState = new EnemyDeathState(this);
    }
    
    public override State InitialState() => idleState;

    protected virtual void Die()
    {
        if (_currentState != deathState)
        {
            SetState(deathState);
        }
    }
    
}