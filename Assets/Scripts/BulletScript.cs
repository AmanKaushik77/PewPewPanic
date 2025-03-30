using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private GameManager gameManager;

    public void SetTarget(Vector3 target, float bulletSpeed)
    {
        targetPosition = target;
        speed = bulletSpeed;
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            // Move toward target without changing rotation
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