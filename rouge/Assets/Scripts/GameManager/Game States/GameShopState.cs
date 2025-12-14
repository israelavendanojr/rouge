using UnityEngine;

public class GameShopState : State
{
    public GameShopState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered Game Shop State");
        // Logic for displaying the shop UI, pausing gameplay, etc.
    }

    public override void Update()
    {
        // Listen for player shop actions or exit button
    }

    public override void Exit()
    {
        Debug.Log("Exiting Game Shop State");
        // Logic to hide the shop UI, unpause if necessary
    }
}