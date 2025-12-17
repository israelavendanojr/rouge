using UnityEngine;
using UnityEngine.InputSystem;

public class GameStartState : State
{
    private InputAction _startAction;
    private GameManager _gameManager; 
    private GameObject _player;

    public GameStartState(StateMachine stateMachine) : base(stateMachine)
    {
        // Store the concrete reference for easy access to SetState
        _gameManager = stateMachine as GameManager;
        _player = _gameManager.GetPlayer();
    }

    public override void Enter()
    {
        _player.GetComponent<MouseFollower>().enabled = false;
        _player.GetComponent<ConsumeSegment>().enabled = false;
        
        _startAction = _gameManager.GetInteractAction().action;
        _startAction.Enable();
        _gameManager.GetStatData().InitializeStats();
    }

    public override void Update()
    {
        
        if (_startAction.WasPressedThisFrame())
        {
            _gameManager.SetState(new GameWaveState(_gameManager));

        }
    }

    public override void Exit()
    {        
        if (_startAction != null)
        {
            _startAction.Disable();
            _startAction.Dispose(); 
        }

        _player.GetComponent<MouseFollower>().enabled = true;
        _player.GetComponent<ConsumeSegment>().enabled = true;
    }
}