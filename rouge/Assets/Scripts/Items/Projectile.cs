using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private string targetTag = "Target";
    
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _direction;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
        _startPosition = transform.position;
        _endPosition = FindNearestTargetPosition();
        _direction = (_endPosition - _startPosition).normalized;
    }

    private void Update()
    {
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        MoveAlongPath();
    }

    private Vector3 FindNearestTargetPosition()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float nearestDistance = float.MaxValue;
        Vector3 nearestPosition = transform.position + transform.up * 10f;
        
        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPosition = target.transform.position;
            }
        }
        
        return nearestPosition;
    }

    private void MoveAlongPath()
    {
        transform.position += _direction * speed * Time.deltaTime;
        
        // Rotate to face direction
        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Destroy if reached or passed endpoint
        if (Vector3.Distance(transform.position, _endPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            OnCollision(other.gameObject);
        }
    }

    private void OnCollision(GameObject other)
    {
        _currentHealth--;
        Debug.Log($"Projectile hit {other.name}. Health remaining: {_currentHealth}");
        
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_startPosition, _endPosition);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_endPosition, 0.3f);
        }
    }
}