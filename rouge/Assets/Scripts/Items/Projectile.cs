using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private string targetTag = "Target";
    
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Projectile Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject exitParticleEffect;

    [Header("Components")]
    private HealthComponent health;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _direction;

    private void Awake()
    {
        health = GetComponent<HealthComponent>();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = FindNearestTargetPosition();
        _direction = (_endPosition - _startPosition).normalized;
    }


    private void Update()
    {
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
        
        if (_direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
    }

    private GameObject FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float nearestDistance = float.MaxValue;
        GameObject nearestTarget = null;
        
        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }
        return nearestTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            HealthComponent targetHealth = other.GetComponent<HealthComponent>();
            if (targetHealth != null)
                targetHealth.TakeDamage(damage);

            health.TakeDamage(1);

            if (health.GetCurrentHealth() <= 0)
            {
                if (exitParticleEffect != null)
                {
                    Instantiate(exitParticleEffect, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
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