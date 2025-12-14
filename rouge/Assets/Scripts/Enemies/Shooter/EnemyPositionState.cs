using UnityEngine;

public class EnemyPositionState : State
{
    private ShooterEnemy enemy;
    private Rigidbody2D rb;
    private Transform target;
    private EnemyShootState shootState;

    private float retreatSpeed;
    private float minDistance;
    private float maxDistance;
    
    private Vector2 moveTarget;
    private bool isRepositioning;

    public EnemyPositionState(ShooterEnemy enemy, Transform target, float speed, float minD, float maxD, float minRepositionD) : base(enemy)
    {
        this.enemy = enemy;
        this.target = target;
        this.rb = enemy.GetRigidbody();
        this.retreatSpeed = speed;
        this.minDistance = minD;
        this.maxDistance = maxD;
    }

    public void SetShootState(EnemyShootState state)
    {
        this.shootState = state;
    }

    public void Reposition(Vector2 newTarget)
    {
        moveTarget = newTarget;
        isRepositioning = true;
    }

    public override void Enter()
    {
        if (!isRepositioning)
        {
            Vector2 direction = ((Vector2)enemy.transform.position - (Vector2)target.position).normalized;
            moveTarget = (Vector2)enemy.transform.position + direction * maxDistance; 
        }
    }

    public override void FixedUpdate()
    {
        if (rb == null || target == null || shootState == null) return;

        enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        float currentDistance = Vector2.Distance(enemy.transform.position, target.position);

        if (isRepositioning)
        {
            HandleRepositioning();
        }
        else if (currentDistance < minDistance)
        {
            HandleRetreat();
        }
        else if (currentDistance > maxDistance)
        {
            HandleMoveToRange();
        }
        else
        {
            enemy.SetState(shootState);
        }

        if (enemy.healthComponent.GetCurrentHealth() <= 0)
        {
            enemy.SetState(new EnemyDeathState(enemy));
        }
    }

    private void HandleRepositioning()
    {
        Vector2 direction = (moveTarget - (Vector2)enemy.transform.position).normalized;
        rb.velocity = direction * retreatSpeed;

        if (Vector2.Distance(enemy.transform.position, moveTarget) < 0.5f)
        {
            isRepositioning = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleRetreat()
    {
        Vector2 direction = ((Vector2)enemy.transform.position - (Vector2)target.position).normalized;
        rb.velocity = direction * retreatSpeed;
    }
    
    private void HandleMoveToRange()
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)enemy.transform.position).normalized;
        rb.velocity = direction * retreatSpeed;
    }

    public override void Exit()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            isRepositioning = false;
        }
    }
}