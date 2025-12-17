using UnityEngine;

public class BossFollowState : State
{
    private BossEnemy boss;
    private Rigidbody2D rb;
    private TargetSystem targetSystem;
    private float moveSpeed;
    private int damage;

    private int targetIndex;
    private float phaseTimer;

    public BossFollowState(BossEnemy boss, float speed, TargetSystem system, int damage)
        : base(boss)
    {
        this.boss = boss;
        this.moveSpeed = speed;
        this.targetSystem = system;
        this.damage = damage;
        rb = boss.GetRigidbody();
    }

    public void SetPhaseDuration(float time)
    {
        phaseTimer = time;
    }

    public override void Enter()
    {
        if (targetSystem.TargetPositions.Length > 0)
            targetIndex = Random.Range(0, targetSystem.TargetPositions.Length);
    }

    public override void Update()
    {
        phaseTimer -= Time.deltaTime;

        if (phaseTimer <= 0f)
        {
            boss.SetState(boss.GetTransitionState());
            return;
        }

        if (boss.healthComponent.GetCurrentHealth() <= 0)
            boss.SetState(new EnemyDeathState(boss));
    }

    public override void FixedUpdate()
    {
        Vector2 targetPos = targetSystem.GetTargetPosition(targetIndex);
        Vector2 dir = (targetPos - (Vector2)boss.transform.position).normalized;
        rb.velocity = dir * moveSpeed;
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;
    }

    public override void OnTriggerEnter(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<HealthComponent>()?.TakeDamage(damage);
        }
    }
}
