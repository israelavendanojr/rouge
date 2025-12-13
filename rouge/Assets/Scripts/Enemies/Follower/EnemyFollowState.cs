using UnityEngine;

public class EnemyFollowState : State
{
    private FollowerEnemy enemy;
    private Rigidbody2D rb;
    private float moveSpeed;
    private TargetSystem targetSystem;
    private int targetIndex;

    public EnemyFollowState(FollowerEnemy enemy, float moveSpeed, TargetSystem system) : base(enemy)
    {
        this.enemy = enemy;
        this.moveSpeed = moveSpeed;
        this.rb = enemy.GetRigidbody();
        this.targetSystem = system;
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

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

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
}