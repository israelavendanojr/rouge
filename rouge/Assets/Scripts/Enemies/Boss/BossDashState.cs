using UnityEngine;

public class BossDashState : State
{
    private BossEnemy boss;
    private Rigidbody2D rb;

    private Transform playerTarget;

    private float dashForce;
    private float dashDuration;
    private float stopDuration;
    private int damage;

    private float phaseTimer;
    private float timer;

    private enum DashPhase { Stop, Dash }
    private DashPhase currentPhase;

    public BossDashState(
        BossEnemy boss,
        float force,
        float dashTime,
        float stopTime,
        int damage
    ) : base(boss)
    {
        this.boss = boss;
        dashForce = force;
        dashDuration = dashTime;
        stopDuration = stopTime;
        this.damage = damage;

        rb = boss.GetRigidbody();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTarget = player != null ? player.transform : null;
    }

    public void SetPhaseDuration(float time)
    {
        phaseTimer = time;
    }

    public override void Enter()
    {
        currentPhase = DashPhase.Stop;
        timer = stopDuration;
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
        {
            boss.SetState(new EnemyDeathState(boss));
        }
    }

    public override void FixedUpdate()
    {
        if (playerTarget == null || rb == null)
            return;

        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            if (currentPhase == DashPhase.Stop)
            {
                DashAtPlayer();
                currentPhase = DashPhase.Dash;
                timer = dashDuration;
            }
            else
            {
                rb.velocity = Vector2.zero;
                currentPhase = DashPhase.Stop;
                timer = stopDuration;
            }
        }
    }

    private void DashAtPlayer()
    {
        Vector2 direction =
            ((Vector2)playerTarget.position - (Vector2)boss.transform.position)
            .normalized;

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

    public override void OnTriggerEnter(Collider2D col)
    {
        if (currentPhase == DashPhase.Dash && col.CompareTag("Player"))
        {
            col.GetComponent<HealthComponent>()?.TakeDamage(damage);
        }
    }
}
