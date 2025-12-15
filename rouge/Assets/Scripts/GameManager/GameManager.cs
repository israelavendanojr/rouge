using UnityEngine;
using UnityEngine.InputSystem;


// Change from MonoBehaviour to StateMachine
public class GameManager : StateMachine 
{
    [SerializeField] private GameObject player;
    public GameObject GetPlayer() => player;
    [SerializeField] private StatData statData;
    public StatData GetStatData() => statData;
    [Header("Start Data")]
    [SerializeField] private InputActionReference interactAction;
    public InputActionReference GetInteractAction() => interactAction;
    
    [Header("Wave Data")]
    private WaveSpawner waveSpawner;
    public WaveSpawner GetWaveSpawner() => waveSpawner;
    [SerializeField] private int wave = 1;
    [SerializeField] private GameEvent onWaveStarted;
    [SerializeField] private float waitTime = 5;
    public float GetWaitTime() => waitTime;
    [SerializeField] private int shopFrequency = 1;
    public int GetShopFrequency() => shopFrequency;
    [SerializeField] private float shopTime = 5;
    public float GetShopTime() => shopTime;
    [SerializeField] private GameObject shopPrefab;
    public GameObject GetShopPrefab() => shopPrefab;
    public int GetLevel() => statData.GetLevel();


    
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

        statData.InitializeStats();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void ReachEndState()
    {
        SetState(new GameEndState(this));
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