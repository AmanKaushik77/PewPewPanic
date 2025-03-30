using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public GameObject enemy;  // The main enemy object (parent).
    public int health = 5;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter fired on " + gameObject.name + " because of: " + other.gameObject.name);

        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected!");
            health--;
            Debug.Log("Health is now: " + health);
            Destroy(other.gameObject);

            if (health <= 0)
            {
                int enemyWave = 0;
                EnemyManager manager = enemy.GetComponent<EnemyManager>();
                GameManager gm = FindFirstObjectByType<GameManager>();

                if (manager != null)
                {
                    // Boss enemies always count as wave 4 (500 points).
                    enemyWave = manager.isBoss ? 4 : (gm != null ? gm.CurrentWave : manager.wave);
                }
                Debug.Log("Enemy destroyed with wave value: " + enemyWave);
                if (gm != null)
                {
                    gm.EnemyDestroyed(enemyWave);
                }
                Destroy(enemy);
            }
        }
    }
}
