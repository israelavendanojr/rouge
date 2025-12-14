using UnityEngine;
using UnityEngine.InputSystem;


// Change from MonoBehaviour to StateMachine
public class GameManager : StateMachine 
{
    [SerializeField] private GameObject player;
    public GameObject GetPlayer() => player;
    [SerializeField] private StatData statData;
    [Header("Start Data")]
    [SerializeField] private InputActionReference startAction;
    public InputActionReference GetStartAction() => startAction;
    
    [Header("Wave Data")]
    private WaveSpawner waveSpawner;
    public WaveSpawner GetWaveSpawner() => waveSpawner;
    [SerializeField] private int wave = 1;
    [SerializeField] private float waitTime = 5;
    public float GetWaitTime() => waitTime;
    [SerializeField] private float shopTime = 5;

    [SerializeField] private GameEvent onWaveStarted;
    
    [Header("Score Data")]
    [SerializeField] private int score = 0;
    [SerializeField] private GameEvent onScoreUpdated;


    public override State InitialState()
    {
        return new GameStartState(this);
    }
    

    void Awake()
    {
        
        if (onWaveStarted != null)
            onWaveStarted.Raise();
        
        if (onScoreUpdated != null)
            onScoreUpdated.Raise();

        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public int GetWave() => wave;
    public int GetScore() => score;

    public void AddScore(int amount = 1)
    {
        score += amount;
        
        if (onScoreUpdated != null)
            onScoreUpdated.Raise();
    }

    public void UpdateWave(int waveNumber)
    {
        wave = waveNumber;
        
        if (onWaveStarted != null)
            onWaveStarted.Raise();
    }
}