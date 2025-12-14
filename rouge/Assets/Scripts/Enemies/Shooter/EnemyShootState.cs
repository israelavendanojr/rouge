using UnityEngine;

public class EnemyShootState : State
{
    private ShooterEnemy enemy;
    private Rigidbody2D rb;
    private Transform target;

    private EnemyPositionState positionState; 

    private float fireRate;
    private int shotsPerVolley;
    
    private float fireTimer;
    private int shotsFired;

    public EnemyShootState(ShooterEnemy enemy, Transform target, float rate, int volley) : base(enemy)
    {
        this.enemy = enemy;
        this.target = target;
        this.rb = enemy.GetRigidbody();
        this.fireRate = rate;
        this.shotsPerVolley = volley;
    }

    public void SetPositionState(EnemyPositionState state)
    {
        this.positionState = state;
    }

    public override void Enter()
    {
        rb.velocity = Vector2.zero; // Stop moving to shoot
        shotsFired = 0;
        fireTimer = 0f;
    }

    public override void Update()
    {
        if (target == null || positionState == null) return;
        
        Vector2 direction = (target.position - enemy.transform.position);

        if (direction.x > 0.05f)
        {
            enemy.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < -0.05f)
        {
            enemy.transform.localScale = new Vector3(-1, 1, 1);
        }

        fireTimer += Time.deltaTime;

        if (shotsFired < shotsPerVolley)
        {
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            Vector2 newTarget = enemy.GetRandomRepositionTarget();
            positionState.Reposition(newTarget);
            enemy.SetState(positionState);
        }
    }

    private void Shoot()
    {
        if (enemy.FireballPrefab == null || enemy.SpawnPoint == null) return; // Safety check
        
        GameObject fireball = GameObject.Instantiate(
            enemy.FireballPrefab, 
            enemy.SpawnPoint.position, // Use the dedicated spawn position
            Quaternion.identity
        );


        shotsFired++;
    }

    public override void Exit()
    {
    }
}