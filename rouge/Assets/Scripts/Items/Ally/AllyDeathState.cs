using UnityEngine;

public class AllyDeathState : State
{
    private AllyController ally;
    private GameManager gameManager; 

    public AllyDeathState(AllyController ally) : base(ally)
    {
        this.ally = ally;
    }

    public override void Enter()
    {


        GameObject.Destroy(ally.gameObject);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}