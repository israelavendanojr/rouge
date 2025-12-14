using UnityEngine;
public class EnemyDeathState : State 
{
    private BaseEnemy enemy;

    public EnemyDeathState(BaseEnemy enemy) : base(enemy.GetComponent<StateMachine>())
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        // To be implemented - play death animation, effects, etc.
        
        enemy.GetSpawnable().SpawnOne();
        enemy.GetGameManager().AddScore(enemy.GetScoreValue() * enemy.GetGameManager().GetWave());
        GameObject.Destroy(enemy.gameObject, 0f); 
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}