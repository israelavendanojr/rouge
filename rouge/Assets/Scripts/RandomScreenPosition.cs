using UnityEngine;

public class RandomScreenPosition : MonoBehaviour
{
    public Camera targetCamera;
    
    [Header("Position Padding")]
    public float paddingX = 50f; 
    
    public float paddingY = 50f;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;

            if (targetCamera == null)
            {
                Debug.LogError("No camera tagged 'MainCamera' found. Please assign a target camera.");
                return;
            }
        }

        SetRandomPosition();
    }

    public void SetRandomPosition()
    {
        float minX = paddingX;
        float maxX = Screen.width - paddingX;
        float minY = paddingY;
        float maxY = Screen.height - paddingY;

        if (minX >= maxX || minY >= maxY)
        {
            minX = 0f; maxX = Screen.width;
            minY = 0f; maxY = Screen.height;
            Debug.LogWarning("Padding exceeds screen size. Using full screen bounds.");
        }

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        float distanceZ = transform.position.z - targetCamera.transform.position.z;
        
        if (targetCamera.orthographic)
        {
            distanceZ = Mathf.Abs(transform.position.z - targetCamera.transform.position.z);
        }
        else
        {
             if (distanceZ <= 0) distanceZ = 10f;
        }

        Vector3 screenPosition = new Vector3(randomX, randomY, distanceZ);
        Vector3 worldPosition = targetCamera.ScreenToWorldPoint(screenPosition);
        transform.position = worldPosition;
    }
}