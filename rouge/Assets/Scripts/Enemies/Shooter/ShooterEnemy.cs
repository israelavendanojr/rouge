using UnityEngine;

public class ShooterEnemy : BaseEnemy
{
    [Header("Behavior Settings")]
    [SerializeField] private float retreatSpeed = 3f;
    [SerializeField] private float minDistanceFromPlayer = 6f;
    [SerializeField] private float maxDistanceFromPlayer = 10f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int shotsPerVolley = 3;

    [Header("Repositioning")]
    [SerializeField] private float minRepositionDistance = 15f;
    
    [Header("Shooting Components")]
    [SerializeField] private Transform spawnPoint;
    
    private Transform playerTarget;
    public GameObject FireballPrefab => fireballPrefab;
    public Transform SpawnPoint => spawnPoint; 

    private EnemyPositionState positionState;
    private EnemyShootState shootState;
    [SerializeField] private SimpleAudioEvent shootAudioEvent;
    public SimpleAudioEvent GetShootAudioEvent() => shootAudioEvent;

    protected override void Awake()
    {
        base.Awake();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTarget = player?.transform;

        if (playerTarget == null)
        {
            Debug.LogError("ShooterEnemy cannot find player with 'Player' tag.");
            return;
        }
        
        if (spawnPoint == null)
        {
            Debug.LogError("ShooterEnemy: SpawnPoint is not assigned in the Inspector!");
        }

        positionState = new EnemyPositionState(this, playerTarget, retreatSpeed, minDistanceFromPlayer, maxDistanceFromPlayer, minRepositionDistance);
        shootState = new EnemyShootState(this, playerTarget, fireRate, shotsPerVolley);
        
        positionState.SetShootState(shootState);
        shootState.SetPositionState(positionState);
    }

    public override State InitialState()
    {
        return positionState;
    }

    public Vector2 GetRandomRepositionTarget()
    {
        Vector2 origin = (Vector2)playerTarget.position;
        float angle = Random.Range(0f, 360f);
        Vector2 offset = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * minRepositionDistance;

        return origin + offset;
    }
}