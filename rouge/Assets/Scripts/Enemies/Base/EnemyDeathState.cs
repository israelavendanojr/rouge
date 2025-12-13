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
        GameObject.Destroy(enemy.gameObject, 0.5f); 
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}