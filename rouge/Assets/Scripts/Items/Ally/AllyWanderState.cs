using UnityEngine;

public class AllyWanderState : State
{
    private AllyController ally;
    private Rigidbody2D rb;
    private SnakeSegments snakeSegments;
    private float moveSpeed;
    private HealthComponent healthComponent;

    private Vector2 wanderTarget;
    private float wanderRadius;
    private float targetReachDistance = 0.5f;
    private float pickupCheckInterval = 0.3f;
    private float pickupCheckTimer;

    public AllyWanderState(AllyController ally, float wanderRadius = 5f) : base(ally)
    {
        this.ally = ally;
        this.rb = ally.GetRigidbody();
        this.snakeSegments = ally.GetSnakeSegments();
        this.moveSpeed = ally.GetMoveSpeed() * 0.6f; // Slower when wandering
        this.healthComponent = ally.GetHealthComponent();
        this.wanderRadius = wanderRadius;
    }

    public override void Enter()
    {
        pickupCheckTimer = 0f;
        PickNewWanderTarget();
    }

    public override void Update()
    {
        // Check for death
        if (healthComponent.GetCurrentHealth() <= 0)
        {
            ally.SetState(new AllyDeathState(ally));
            return;
        }

        // Periodically check for pickups
        pickupCheckTimer -= Time.deltaTime;
        if (pickupCheckTimer <= 0f)
        {
            pickupCheckTimer = pickupCheckInterval;
            
            GameObject nearestPickup = FindNearestPickup();
            if (nearestPickup != null)
            {
                ally.SetState(ally.GetCollectState());
                return;
            }
        }

        // If we have segments, switch to position state
        if (snakeSegments.GetSegmentCount() > 0)
        {
            ally.SetState(ally.GetPositionState());
            return;
        }
    }

    public override void FixedUpdate()
    {
        // Check if we reached our wander target
        float distanceToTarget = Vector2.Distance(ally.transform.position, wanderTarget);
        
        if (distanceToTarget < targetReachDistance)
        {
            PickNewWanderTarget();
        }

        // Move toward wander target
        Vector2 direction = (wanderTarget - (Vector2)ally.transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void PickNewWanderTarget()
    {
        // Generate a random point within wanderRadius of current position
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        wanderTarget = (Vector2)ally.transform.position + randomDirection;
        
        // Optional: Clamp to screen bounds if needed
        // You can add screen boundary checks here if you want
    }

    private GameObject FindNearestPickup()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        
        if (pickups.Length == 0)
            return null;

        float nearestDistance = float.MaxValue;
        GameObject nearest = null;

        foreach (GameObject pickup in pickups)
        {
            float distance = Vector2.Distance(ally.transform.position, pickup.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = pickup;
            }
        }

        return nearest;
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;
    }
}