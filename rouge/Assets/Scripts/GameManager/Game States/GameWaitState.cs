using UnityEngine;

public class GameWaitState : State
{
    private float waitTime;               
    private GameManager _gameManager;

    public GameWaitState(StateMachine stateMachine) : base(stateMachine)
    {
        _gameManager = stateMachine as GameManager;
        waitTime = _gameManager.GetWaitTime(); 
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        waitTime -= Time.deltaTime;

        if (waitTime <= 0f)
        {
            waitTime = 0f;
            _gameManager.SetState(new GameWaveState(_gameManager));
        }
    }

    public override void Exit()
    {
        
    }
}
