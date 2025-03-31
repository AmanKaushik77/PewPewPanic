using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public GameObject enemy;
    public int health = 5;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter fired on " + gameObject.name + " because of: " + other.gameObject.name);
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected!");
            health--;
            EnemyManager manager = enemy.GetComponent<EnemyManager>();
            if (manager != null && manager.isBoss)
            {
                Debug.Log("Boss Ship Health: " + health);
            }
            else
            {
                Debug.Log("Health is now: " + health);
            }
            Destroy(other.gameObject);
            if (health <= 0)
            {
                if (manager != null && manager.isBoss)
                {
                    GameManager gm = FindObjectOfType<GameManager>();
                    if (gm != null)
                    {
                        gm.BossDestroyed();
                    }
                }
                else
                {
                    int enemyWave = 0;
                    if (manager != null)
                        enemyWave = manager.isBoss ? 4 : FindObjectOfType<GameManager>().CurrentWave;
                    Debug.Log("Enemy destroyed with wave value: " + enemyWave);
                    GameManager gm = FindObjectOfType<GameManager>();
                    if (gm != null)
                        gm.EnemyDestroyed(enemyWave);
                }
                Destroy(enemy);
            }
        }
    }
}
