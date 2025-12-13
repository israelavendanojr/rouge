using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    [SerializeField] private float rangeVal = 3f;
    [SerializeField] private int targetCount = 3;
    [SerializeField] private float seconds = 2f; 

    public Vector2[] TargetPositions { get; private set; }

    private float _timer;

    private void Awake()
    {
        TargetPositions = new Vector2[targetCount];
        MoveTargets();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= seconds)
        {
            MoveTargets();
            _timer = 0f;
        }
    }

    private void MoveTargets()
    {
        for (int i = 0; i < TargetPositions.Length; i++)
        {
            TargetPositions[i] = GenerateRandomTarget();
        }
    }

    private Vector2 GenerateRandomTarget()
    {
        Vector2 origin = transform.position;
        float x = Random.Range(-rangeVal, rangeVal);
        float y = Random.Range(-rangeVal, rangeVal);
        return origin + new Vector2(x, y);
    }

    public Vector2 GetTargetPosition(int index)
    {
        if (TargetPositions == null || TargetPositions.Length == 0) return transform.position;

        return TargetPositions[index % TargetPositions.Length];
    }

    private void OnDrawGizmosSelected()
    {
        if (TargetPositions != null)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                Gizmos.DrawWireSphere(TargetPositions[i], 0.3f);
                Gizmos.DrawLine(transform.position, TargetPositions[i]);
            }
        }
    }
}