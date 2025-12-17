using UnityEngine;
using UnityEngine.InputSystem;

public class GameWinState : State
{
    private InputAction _interactAction;
    private GameManager _gameManager;
    private GameObject _player;

    public GameWinState(StateMachine stateMachine) : base(stateMachine)
    {
        _gameManager = stateMachine as GameManager;
        _player = _gameManager.GetPlayer();
    }

    public override void Enter()
    {
        // Disable player control
        _player.GetComponent<MouseFollower>().enabled = false;
        _player.GetComponent<ConsumeSegment>().enabled = false;

        // Enable interact action for next input
        _interactAction = _gameManager.GetInteractAction().action;
        _interactAction.Enable();

        _gameManager.LoadNextScene(1f);
    }

    public override void Update()
    {
        // Uncomment if you want the interact action to restart or continue
        // if (_interactAction.WasPressedThisFrame())
        // {
        //     _gameManager.RestartGame();
        // }\
        
        
    }

    public override void Exit()
    {
        if (_interactAction != null)
        {
            _interactAction.Disable();
            _interactAction.Dispose();
        }

        // Re-enable player control
        _player.GetComponent<MouseFollower>().enabled = true;
        _player.GetComponent<ConsumeSegment>().enabled = true;
    }
}
