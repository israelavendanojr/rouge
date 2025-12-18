using UnityEngine;

public class AllyController : StateMachine
{
    [Header("Components")]
    private Rigidbody2D rb;
    private SnakeSegments snakeSegments;
    private HealthComponent healthComponent;
    public HealthComponent GetHealthComponent() => healthComponent;
    
    public Rigidbody2D GetRigidbody() => rb;
    public SnakeSegments GetSnakeSegments() => snakeSegments;

    [Header("States")]
    private AllyCollectState collectState;
    private AllyPositionState positionState;
    private AllyConsumeState consumeState;
    private AllyWanderState wanderState;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float consumeRange = 8f;
    [SerializeField] private float consumeInterval = 0.5f;
    [SerializeField] private float wanderRadius = 5f;

    public float GetMoveSpeed() => moveSpeed;
    public float GetConsumeRange() => consumeRange;
    public float GetConsumeInterval() => consumeInterval;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        snakeSegments = GetComponent<SnakeSegments>();
        healthComponent = GetComponent<HealthComponent>();

        if (rb == null)
        {
            Debug.LogError("AllyController requires a Rigidbody2D component!");
        }

        if (snakeSegments == null)
        {
            Debug.LogError("AllyController requires a SnakeSegments component!");
        }

        collectState = new AllyCollectState(this);
        positionState = new AllyPositionState(this);
        consumeState = new AllyConsumeState(this);
        wanderState = new AllyWanderState(this, wanderRadius);
        
    }
    
    private void OnDestroy()
    {
    }

    public override State InitialState() => wanderState; // Start with wandering

    public AllyCollectState GetCollectState() => collectState;
    public AllyPositionState GetPositionState() => positionState;
    public AllyConsumeState GetConsumeState() => consumeState;
    public AllyWanderState GetWanderState() => wanderState;
}