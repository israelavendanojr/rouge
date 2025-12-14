using UnityEngine;

public class DasherEnemy : BaseEnemy
{
    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float stopDuration = 1.5f; 
    [SerializeField] private int damage = 1;
    
    [SerializeField] private TargetSystem targetSystem; 

    private EnemyDashState dashState;

    protected override void Awake()
    {
        base.Awake();

        TargetSystem targetSystem = GameObject.FindGameObjectWithTag("Player")?.GetComponent<TargetSystem>();
        dashState = new EnemyDashState(this, targetSystem, dashForce, dashDuration, stopDuration, damage);
    }

    public override State InitialState() => dashState;
}