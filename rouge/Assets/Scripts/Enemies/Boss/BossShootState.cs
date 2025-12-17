using UnityEngine;

public class BossShootState : State
{
    private BossEnemy boss;
    private Rigidbody2D rb;
    private Transform target;

    private float fireRate;
    private int volleyCount;

    private float fireTimer;
    private float phaseTimer;
    private int shotsFired;
    private SimpleAudioEvent shootSound;


    public BossShootState(
        BossEnemy boss,
        Transform target,
        float fireRate,
        int volley,
        SimpleAudioEvent shootSound
    ) : base(boss)
    {
        this.boss = boss;
        this.target = target;
        this.fireRate = fireRate;
        volleyCount = volley;
        rb = boss.GetRigidbody();
        this.shootSound = shootSound;
    }

    public void SetPhaseDuration(float time)
    {
        phaseTimer = time;
    }

    public override void Enter()
    {
        rb.velocity = Vector2.zero;
        fireTimer = 0f;
        shotsFired = 0;
    }

    public override void Update()
    {
        phaseTimer -= Time.deltaTime;

        if (phaseTimer <= 0f)
        {
            boss.SetState(boss.GetTransitionState());
            return;
        }

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }

        if (boss.healthComponent.GetCurrentHealth() <= 0)
            boss.SetState(new EnemyDeathState(boss));
    }

    private void Shoot()
    {
        if (boss.FireballPrefab == null || boss.SpawnPoint == null)
            return;

        GameObject.Instantiate(
            boss.FireballPrefab,
            boss.SpawnPoint.position,
            Quaternion.identity
        );

        shotsFired++;
        shootSound.Play();
    }
}
