using UnityEngine;

public class FollowerEnemy : BaseEnemy
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private EnemyFollowState followState;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

        TargetSystem targetSystem = GameObject.FindGameObjectWithTag("Player")?.GetComponent<TargetSystem>();
        followState = new EnemyFollowState(this, moveSpeed, targetSystem);
    }

    public override State InitialState() => followState;

    
}