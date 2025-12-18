using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SimpleSceneController : MonoBehaviour
{
    // ================================
    // PUBLIC API
    // ================================

    public void RestartScene()
    {
        RestartScene(0f);
    }

    public void RestartScene(float delay)
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().buildIndex, delay));
    }

    public void LoadNextScene()
    {
        LoadNextScene(0f);
    }

    public void LoadNextScene(float delay)
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("SimpleSceneController: No next scene in Build Settings.");
            return;
        }

        StartCoroutine(LoadSceneRoutine(nextIndex, delay));
    }

    public void LoadSceneByIndex(int index)
    {
        LoadSceneByIndex(index, 0f);
    }

    public void LoadSceneByIndex(int index, float delay)
    {
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"SimpleSceneController: Invalid scene index {index}");
            return;
        }

        StartCoroutine(LoadSceneRoutine(index, delay));
    }

    // ================================
    // INTERNAL
    // ================================

    private IEnumerator LoadSceneRoutine(int index, float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(index);
    }
}
