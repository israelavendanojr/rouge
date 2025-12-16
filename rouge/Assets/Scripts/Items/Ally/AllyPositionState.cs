using UnityEngine;

public class AllyPositionState : State
{
    private AllyController ally;
    private Rigidbody2D rb;
    private SnakeSegments snakeSegments;
    private float moveSpeed;
    private float consumeRange;

    private GameObject targetEnemy;
    private HealthComponent healthComponent;

    public AllyPositionState(AllyController ally) : base(ally)
    {
        this.ally = ally;
        this.rb = ally.GetRigidbody();
        this.snakeSegments = ally.GetSnakeSegments();
        this.moveSpeed = ally.GetMoveSpeed();
        this.consumeRange = ally.GetConsumeRange();
        this.healthComponent = ally.GetHealthComponent();
    }

    public override void Enter()
    {
        FindNearestTarget();
    }

    public override void FixedUpdate()
    {
        if (snakeSegments.GetSegmentCount() == 0)
        {
            ally.SetState(ally.GetCollectState());
            return;
        }

        if (targetEnemy == null)
        {
            FindNearestTarget();
            
            if (targetEnemy == null)
            {
                rb.velocity = Vector2.zero;
                return;
            }
        }

        float distance = Vector2.Distance(ally.transform.position, targetEnemy.transform.position);

        if (distance <= consumeRange)
        {
            rb.velocity = Vector2.zero;
            ally.SetState(ally.GetConsumeState());
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        
        if (targets.Length == 0)
        {
            targetEnemy = null;
            return;
        }

        float nearestDistance = float.MaxValue;
        GameObject nearest = null;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(ally.transform.position, target.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = target;
            }
        }

        targetEnemy = nearest;
    }

    public override void Update()
    {
        if (healthComponent.GetCurrentHealth() <= 0)
        {
            ally.SetState(new AllyDeathState(ally));
        } 
    }

    private void MoveTowardsTarget()
    {
        if (targetEnemy == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(ally.transform.position, targetEnemy.transform.position);

        if (distance > consumeRange)
        {
            Vector2 direction = ((Vector2)targetEnemy.transform.position - rb.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;
    }
}