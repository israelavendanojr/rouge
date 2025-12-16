using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private string targetTag = "Target";
    [SerializeField] private float retargetInterval = 0.2f;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float predictionStrength = 0.5f;

    [Header("Projectile Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject exitParticleEffect;

    private HealthComponent health;

    private Transform lockedTarget;
    private Rigidbody2D targetRb;
    private Vector2 direction;

    private float retargetTimer;

    private void Awake()
    {
        health = GetComponent<HealthComponent>();
    }

    private void Start()
    {
        AcquireNewTarget();
        direction = transform.up;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        HandleTargeting();
        UpdateDirection();
        Move();
        Rotate();
    }

    private void HandleTargeting()
    {
        if (lockedTarget == null)
        {
            retargetTimer -= Time.deltaTime;
            if (retargetTimer <= 0f)
            {
                AcquireNewTarget();
                retargetTimer = retargetInterval;
            }
        }
    }

    private void AcquireNewTarget()
    {
        lockedTarget = FindRandomTarget();
        targetRb = lockedTarget != null
            ? lockedTarget.GetComponent<Rigidbody2D>()
            : null;
    }

    private Transform FindRandomTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        if (targets.Length == 0)
            return null;

        // Try a few random picks to avoid nulls
        for (int i = 0; i < 5; i++)
        {
            GameObject candidate = targets[Random.Range(0, targets.Length)];
            if (candidate != null)
                return candidate.transform;
        }

        return null;
    }

    private void UpdateDirection()
    {
        if (lockedTarget == null)
            return;

        Vector2 targetPosition = lockedTarget.position;

        if (targetRb != null)
            targetPosition += targetRb.velocity * predictionStrength;

        Vector2 desiredDirection =
            (targetPosition - (Vector2)transform.position).normalized;

        direction = Vector2.Lerp(direction, desiredDirection, Time.deltaTime * 5f).normalized;
    }

    private void Move()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (direction == Vector2.zero)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
            return;

        HealthComponent targetHealth = other.GetComponent<HealthComponent>();
        if (targetHealth != null)
            targetHealth.TakeDamage(damage);

        health?.TakeDamage(1);

        if (health != null && health.GetCurrentHealth() <= 0)
            KillProjectile();
    }

    private void KillProjectile()
    {
        if (exitParticleEffect != null)
            Instantiate(exitParticleEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
