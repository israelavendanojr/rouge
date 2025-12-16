using UnityEngine;

public class AllyConsumeState : State
{
    private AllyController ally;
    private Rigidbody2D rb;
    private SnakeSegments snakeSegments;
    private float consumeInterval;

    private float consumeTimer;

    public AllyConsumeState(AllyController ally) : base(ally)
    {
        this.ally = ally;
        this.rb = ally.GetRigidbody();
        this.snakeSegments = ally.GetSnakeSegments();
        this.consumeInterval = ally.GetConsumeInterval();
    }

    public override void Enter()
    {
        rb.velocity = Vector2.zero;
        consumeTimer = 0f;
    }

    public override void Update()
    {
        if (snakeSegments.GetSegmentCount() == 0)
        {
            ally.SetState(ally.GetCollectState());
            return;
        }

        consumeTimer += Time.deltaTime;

        if (consumeTimer >= consumeInterval)
        {
            ConsumeSegment();
            consumeTimer = 0f;

            if (snakeSegments.GetSegmentCount() == 0)
            {
                ally.SetState(ally.GetCollectState());
            }
        }
    }

    private void ConsumeSegment()
    {
        if (snakeSegments.GetSegmentCount() > 0)
        {
            snakeSegments.ConsumeLastSegment();
        }
    }

    public override void Exit()
    {
        consumeTimer = 0f;
    }
}