using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 targetPosition; // Used for bullets (static) and lasers (initial fallback)
    private Transform targetEnemy;  // Used for lasers to track moving enemies
    private float speed;
    private GameManager gameManager;
    private bool isLaser; // Flag to distinguish laser behavior

    public void SetTarget(Vector3 target, float bulletSpeed, Transform enemy = null)
    {
        targetPosition = target;
        speed = bulletSpeed;
        targetEnemy = enemy; // Set for lasers, null for bullets
        isLaser = (gameManager != null && gameManager.HasLaser); // Determine if this is a laser
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        // Lasers track the enemy's current position if available
        if (isLaser && targetEnemy != null)
        {
            targetPosition = targetEnemy.position; // Update to enemy's live position
        }

        if (targetPosition != Vector3.zero)
        {
            // Move toward target (static for bullets, updated for lasers)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            if (enemyScript != null)
            {
                int damage = (gameManager != null && gameManager.HasLaser) ? 5 : 1;
                enemyScript.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}