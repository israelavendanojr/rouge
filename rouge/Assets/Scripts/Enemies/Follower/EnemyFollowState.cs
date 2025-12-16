using UnityEngine;

public class EnemyFollowState : State
{
    private FollowerEnemy enemy;
    private Rigidbody2D rb;
    private float moveSpeed;
    private TargetSystem targetSystem;
    private int targetIndex;
    private int damage;

    public EnemyFollowState(FollowerEnemy enemy, float moveSpeed, TargetSystem system, int damage) : base(enemy)
    {
        this.enemy = enemy;
        this.moveSpeed = moveSpeed;
        this.rb = enemy.GetRigidbody();
        this.targetSystem = system;
        this.damage = damage;
    }

    public override void Enter()
    {
        if (targetSystem != null && targetSystem.TargetPositions != null)
        {
            targetIndex = Random.Range(0, targetSystem.TargetPositions.Length);
        }
    }

    public override void FixedUpdate()
    {
        if (rb == null || targetSystem == null) return;

        Vector2 currentTargetPos = targetSystem.GetTargetPosition(targetIndex);
        float distance = Vector2.Distance(enemy.transform.position, currentTargetPos);

        if (distance > 0.1f)
        {
            Vector2 direction = (currentTargetPos - (Vector2)enemy.transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        // Check for death
        if (enemy.healthComponent.GetCurrentHealth() <= 0)
        {
            enemy.SetState(new EnemyDeathState(enemy));
        }
    }

    public override void Exit()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Player") || collision.CompareTag("Ally"))
        {
            HealthComponent targetHealth = other.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

        }    
    }
}