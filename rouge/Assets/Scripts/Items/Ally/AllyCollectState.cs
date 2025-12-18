using UnityEngine;

public class AllyCollectState : State
{
    private AllyController ally;
    private Rigidbody2D rb;
    private SnakeSegments snakeSegments;
    private float moveSpeed;

    private GameObject targetPickup;
    private HealthComponent healthComponent;
    private float noPickupTimer;
    private float noPickupThreshold = 1f; // Switch to wander after 1 second with no pickup


    public AllyCollectState(AllyController ally) : base(ally)
    {
        this.ally = ally;
        this.rb = ally.GetRigidbody();
        this.snakeSegments = ally.GetSnakeSegments();
        this.moveSpeed = ally.GetMoveSpeed();
        this.healthComponent = ally.GetHealthComponent();
    }

    public override void Enter()
    {
        FindNearestPickup();
        noPickupTimer = 0f;
    }

    public override void FixedUpdate()
    {
        if (targetPickup == null)
        {
            FindNearestPickup();
            
            if (targetPickup == null)
            {
                rb.velocity = Vector2.zero;
                
                // If we have segments, go to position state
                if (snakeSegments.GetSegmentCount() > 0)
                {
                    ally.SetState(ally.GetPositionState());
                    return;
                }
                
                // Otherwise wait a bit, then switch to wander
                noPickupTimer += Time.fixedDeltaTime;
                if (noPickupTimer >= noPickupThreshold)
                {
                    ally.SetState(ally.GetWanderState());
                }
                return;
            }
            else
            {
                noPickupTimer = 0f; // Reset timer when pickup found
            }
        }

        MoveTowardsPickup();

        if (snakeSegments.GetSegmentCount() > 0)
        {
            ally.SetState(ally.GetPositionState());
        }
    }
    
    public override void Update()
    {
        if (healthComponent.GetCurrentHealth() <= 0)
        {
            ally.SetState(new AllyDeathState(ally));
        } 
    }

    private void FindNearestPickup()
    {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        
        if (pickups.Length == 0)
        {
            targetPickup = null;
            return;
        }

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

        targetPickup = nearest;
    }

    private void MoveTowardsPickup()
    {
        if (targetPickup == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)targetPickup.transform.position - rb.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;
        targetPickup = null;
        noPickupTimer = 0f;
    }
}