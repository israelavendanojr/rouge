using UnityEngine;

public class FollowerEnemy : BaseEnemy
{
    [Header("")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int damage = 1;

    private EnemyFollowState followState;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponent>();

        TargetSystem targetSystem = GameObject.FindGameObjectWithTag("Player")?.GetComponent<TargetSystem>();
        followState = new EnemyFollowState(this, moveSpeed, targetSystem, damage);
    }

    public override State InitialState() => followState;

}