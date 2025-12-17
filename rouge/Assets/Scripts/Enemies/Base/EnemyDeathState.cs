using UnityEngine;

public class EnemyDeathState : State
{
    private BaseEnemy enemy;
    private Rigidbody2D rb;

    private float deathMoveSpeed = 6f;
    private float lifetime = 5f;
    SimpleAudioEvent deathAudioEvent;

    public EnemyDeathState(BaseEnemy enemy) 
        : base(enemy.GetComponent<StateMachine>())
    {
        this.enemy = enemy;
        rb = enemy.GetComponent<Rigidbody2D>();
        deathAudioEvent = enemy.GetDeathAudioEvent();
    }

    public override void Enter()
    {
        // Play death sound
        deathAudioEvent?.Play();

        // Rewards
        enemy.GetSpawnable().SpawnOne();
        enemy.GetGameManager().AddScore(
            enemy.GetScoreValue() * enemy.GetGameManager().GetWave()
        );

        // Visuals — spawn present as a child so it looks like enemy is carrying it
        GameObject present = GameObject.Instantiate(
            enemy.GetHappyPresentPrefab(),
            enemy.transform.position,
            Quaternion.identity,
            enemy.transform
        );

        // Offset to the right of the enemy sprite
        Vector3 offset = new Vector3(0.5f, 0f, 0f); 
        present.transform.localPosition += offset;


        MoveAwayFromPlayer();

        // Destroy after moving off screen
        GameObject.Destroy(enemy.gameObject, lifetime);
    }

    private void MoveAwayFromPlayer()
    {
        if (rb == null)
            return;

        GameObject player = enemy.GetGameManager().GetPlayer();
        if (player == null)
            return;

        Vector2 direction =
            ((Vector2)enemy.transform.position - (Vector2)player.transform.position)
            .normalized;

        rb.velocity = direction * deathMoveSpeed;
    }

    public override void Exit()
    {
        if (rb != null)
            rb.velocity = Vector2.zero;
    }
}
