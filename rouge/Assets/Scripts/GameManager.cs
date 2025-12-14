using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private StatData statData;
    
    [Header("Wave Data")]
    [SerializeField] private int wave = 1;
    [SerializeField] private GameEvent onWaveStarted;
    
    [Header("Score Data")]
    [SerializeField] private int score = 0;
    [SerializeField] private GameEvent onScoreUpdated;

    public int GetWave() => wave;
    public int GetScore() => score;

    void Start()
    {
        // Raise initial events to update UI
        if (onWaveStarted != null)
            onWaveStarted.Raise();
        
        if (onScoreUpdated != null)
            onScoreUpdated.Raise();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void AddScore(int amount = 1)
    {
        score += amount;
        
        if (onScoreUpdated != null)
            onScoreUpdated.Raise();
    }

    public void NextWave(int waveNumber)
    {
        wave = waveNumber;
        
        if (onWaveStarted != null)
            onWaveStarted.Raise();
    }
}