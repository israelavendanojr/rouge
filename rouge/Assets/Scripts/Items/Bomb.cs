using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float fuseTime = 3f;
    [SerializeField] private int damage = 1;

    [SerializeField] private CircleCollider2D explosionCollider;
    [SerializeField] private string targetTag = "Target";
    [SerializeField] SimpleAudioEvent placeSound;
    [SerializeField] SimpleAudioEvent explosionSound;

    [SerializeField] private GameObject particles;

    
    private float _spawnTime;

    private void Start()
    {
        _spawnTime = Time.time;
        
        if (explosionCollider != null)
        {
            explosionCollider.enabled = false;
        }

        placeSound.Play();
    }

    private void Update()
    {
        if (Time.time - _spawnTime >= fuseTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        
        if (particles != null)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        
        if (explosionCollider != null)
        {
            explosionCollider.enabled = true;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position, 
                explosionCollider.radius
            );
            
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag(targetTag))
                {
                    // Debug.Log($"Bomb hit: {hit.gameObject.name}");
                    hit.GetComponent<HealthComponent>()?.TakeDamage(damage);
                }
            }
        }
        
        explosionSound?.Play();
        Destroy(gameObject);
    }
}