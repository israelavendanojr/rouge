using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 8f; 

    void Start()
    {
        Destroy(gameObject, delayInSeconds);
    }
}