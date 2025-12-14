using UnityEngine;

public class EnemyDashState : State
{
    private DasherEnemy enemy;
    private Rigidbody2D rb;
    
    private TargetSystem targetSystem; 
    private int targetIndex; 
    private float dashForce;
    private float dashDuration;
    private float stopDuration;
    private int damage;

    private float timer;
    private StatePhase currentPhase;

    private enum StatePhase { Stop, Dashing }

    public EnemyDashState(DasherEnemy enemy, TargetSystem system, float dashForce, float dashDuration, float stopDuration, int damage) : base(enemy)
    {
        this.enemy = enemy;
        this.targetSystem = system; 
        this.dashForce = dashForce;
        this.dashDuration = dashDuration;
        this.stopDuration = stopDuration;
        this.damage = damage;
        this.rb = enemy.GetRigidbody();
    }

    public override void Enter()
    {
        currentPhase = StatePhase.Stop; 
        timer = 0f;
        
        SelectNewTarget();
    }

    private void SelectNewTarget()
    {
        if (targetSystem != null && targetSystem.TargetPositions != null && targetSystem.TargetPositions.Length > 0)
        {
            targetIndex = Random.Range(0, targetSystem.TargetPositions.Length);
        }
    }

    public override void FixedUpdate()
    {
        if (enemy.healthComponent.GetCurrentHealth() <= 0)
        {
            enemy.SetState(new EnemyDeathState(enemy));
            return;
        }
        if (targetSystem == null || rb == null) return; 

        enemy.transform.rotation = Quaternion.Euler(0, 0, 0);

        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            if (currentPhase == StatePhase.Dashing)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                currentPhase = StatePhase.Stop;
                timer = stopDuration;
                
                SelectNewTarget();
            }
            else 
            {
                StartDash();
                currentPhase = StatePhase.Dashing;
                timer = dashDuration;
            }
        }
    }

    private void StartDash()
    {
        if (targetSystem == null) return;
        
        Vector2 currentTargetPos = targetSystem.GetTargetPosition(targetIndex);
        
        Vector2 direction = (currentTargetPos - (Vector2)enemy.transform.position).normalized;

        rb.velocity = Vector2.zero; 
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
    }
    
    public override void Exit()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentPhase == StatePhase.Dashing)
            {
                HealthComponent targetHealth = collision.GetComponent<HealthComponent>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }
            }
        }
    }
}