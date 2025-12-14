using UnityEngine;

public class GameEndState : State
{
    public GameEndState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered Game End State");
        // Logic for displaying the game over screen, final score, high score submission
    }

    public override void Update()
    {
        // Listen for "Restart" or "Quit" input
    }

    public override void Exit()
    {
        Debug.Log("Exiting Game End State");
        // Any final cleanup before loading a new scene
    }
}