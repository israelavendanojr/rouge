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
        followState = new EnemyFollowState(this, moveSpeed);
    }

    public override State InitialState() => followState;

    
}