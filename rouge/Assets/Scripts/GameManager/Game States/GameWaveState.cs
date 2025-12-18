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
            BossEnemy BossEnemy = GameObject.FindObjectOfType<BossEnemy>();
            if (!waveSpawner.WavesRemaining())
            {
                if (BossEnemy == null)
                    _gameManager.SetState(new GameWinState(_gameManager));
            }
            else
            {
                bool isShopTime = (_gameManager.GetStatData().waveNumber % _gameManager.GetShopFrequency() == 0) ? true : false;

                if (isShopTime)
                    _gameManager.SetState(new GameShopState(_gameManager));
                else
                    _gameManager.SetState(new GameWaitState(_gameManager));
            }

                
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