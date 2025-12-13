using UnityEngine;

public class EnemyIdleState : State
{
    private BaseEnemy enemy;

    public EnemyIdleState(BaseEnemy enemy) : base(enemy.GetComponent<StateMachine>())
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}