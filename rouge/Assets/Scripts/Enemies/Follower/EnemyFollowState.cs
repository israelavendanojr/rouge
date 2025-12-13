using UnityEngine;

public class EnemyFollowState : State
{
    private FollowerEnemy enemy;
    private GameObject player;
    private Rigidbody2D rb;
    private float moveSpeed;

    public EnemyFollowState(FollowerEnemy enemy, float moveSpeed) : base(enemy)
    {
        this.enemy = enemy;
        this.moveSpeed = moveSpeed;
        this.rb = enemy.GetRigidbody();
    }

    public override void Enter()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No GameObject with 'Player' tag found!");
        }
    }

    public override void Update()
    {
        if (player == null || rb == null) return;

        Vector2 direction = (player.transform.position - enemy.transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Rotate to face movement direction
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    public override void Exit()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}