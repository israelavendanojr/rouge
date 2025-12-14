using System.Diagnostics;
using UnityEngine;

public class GameWaveState : State
{
    WaveSpawner waveSpawner;
    private GameManager _gameManager; 
    public GameWaveState(StateMachine stateMachine) : base(stateMachine)
    {
        // Store the concrete reference for easy access to SetState
        _gameManager = stateMachine as GameManager;
        waveSpawner = _gameManager.GetWaveSpawner();
    }

    public override void Enter()
    {
        // Debug.Log("Entered Game Wave State");
        // Logic for spawning enemies, starting wave timer, etc.
        waveSpawner.StartWave();
    }

    public override void Update()
    {
        // Logic for active wave gameplay

        if (waveSpawner.IsWaveComplete())
        {
            waveSpawner.currentWaveIndex++;
            if (waveSpawner.WavesRemaining())
                _gameManager.SetState(new GameWaitState(_gameManager));
            else
                UnityEngine.Debug.Log("All waves completed! Game Over.");
                // _gameManager.ChangeState(new GameOverState(_gameManager));
        }
    }

    public override void FixedUpdate()
    {
        // Physics updates for enemies/player during the wave
    }

    public override void Exit()
    {
        
        // Logic for tallying remaining enemies, preparing for next phase
    }
}