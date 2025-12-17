using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Phase Settings")]
    [SerializeField] private float phaseChangeHealthPercent = 0.5f;
    [SerializeField] private float minPhaseTime = 5f;
    [SerializeField] private float maxPhaseTime = 10f;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private int followDamage = 2;

    [Header("Dash")]
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float dashStopDuration = 2f;
    [SerializeField] private int dashDamage = 3;

    [Header("Shoot")]
    [SerializeField] private float shootFireRate = 0.3f;
    [SerializeField] private int shootVolley = 5;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SimpleAudioEvent shootSound;


    private Transform player;
    private TargetSystem targetSystem;

    private BossTransitionState transition;
    private BossFollowState follow;
    private BossDashState dash;
    private BossShootState shoot;

    public GameObject FireballPrefab => fireballPrefab;
    public Transform SpawnPoint => spawnPoint;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        targetSystem = player.GetComponent<TargetSystem>();

        follow = new BossFollowState(this, followSpeed, targetSystem, followDamage);
        dash = new BossDashState(this, dashForce, dashDuration, dashStopDuration, dashDamage);
        shoot = new BossShootState(this, player, shootFireRate, shootVolley, shootSound);
        transition = new BossTransitionState(this, minPhaseTime, maxPhaseTime);
    }

    public override State InitialState() => transition;

    public BossTransitionState GetTransitionState() => transition;
    public BossFollowState GetFollowState() => follow;
    public BossDashState GetDashState() => dash;
    public BossShootState GetShootState() => shoot;


    public float GetPhaseChangeHealthPercent() => phaseChangeHealthPercent;
}
