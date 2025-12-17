using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


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
    [SerializeField] private GameEvent onShopEnd;
    public GameEvent GetOnShopEnd() => onShopEnd;

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
        RestartGame(0f);
    }

    public void RestartGame(float delay)
    {
        StartCoroutine(RestartGameRoutine(delay));
    }

    private IEnumerator RestartGameRoutine(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void LoadNextScene()
    {
        LoadNextScene(0f);
    }

    public void LoadNextScene(float delay)
    {
        StartCoroutine(LoadNextSceneRoutine(delay));
    }

    private IEnumerator LoadNextSceneRoutine(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            nextIndex = 0; // Wrap around to first scene if at the end

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextIndex);
    }
    public void ReachEndState()
    {
        SetState(new GameEndState(this));
    }

    public void ReachWinState()
    {
        SetState(new GameWinState(this));
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