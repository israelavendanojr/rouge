using UnityEngine;
using UnityEngine.InputSystem;

public class GameEndState : State
{
    private InputAction _respawnAction;
    private GameManager _gameManager;
    private GameObject _player;

    public GameEndState(StateMachine stateMachine) : base(stateMachine)
    {
        _gameManager = stateMachine as GameManager;
        _player = _gameManager.GetPlayer();
    }

    public override void Enter()
    {
        // Disable player control
        _player.GetComponent<MouseFollower>().enabled = false;
        _player.GetComponent<ConsumeSegment>().enabled = false;

        
        _respawnAction = _gameManager.GetInteractAction().action;
        _respawnAction.Enable();

    }

    public override void Update()
    {
        // if (_respawnAction.WasPressedThisFrame())
        // {
        //     _gameManager.RestartGame();
            
        // }
    }

    public override void Exit()
    {
        if (_respawnAction != null)
        {
            _respawnAction.Disable();
            _respawnAction.Dispose();
        }

        // Re-enable player control
        _player.GetComponent<MouseFollower>().enabled = true;
        _player.GetComponent<ConsumeSegment>().enabled = true;
    }
}
