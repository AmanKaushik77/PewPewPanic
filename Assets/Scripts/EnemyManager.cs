using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;         // The main enemy object (parent).
    public int health = 5;
    public float moveSpeed = 30f;    // Fixed speed for bosses.
    public Vector3 moveDirection = Vector3.forward;

    public bool isBoss = true;
    public int wave = 1;

    private void Start()
    {
        if (enemy == null && transform.parent != null)
        {
            enemy = transform.parent.gameObject;
        }

        if (!isBoss)
        {
            moveSpeed = Random.Range(5f, 20f);
            Debug.Log("Assigned random moveSpeed: " + moveSpeed);
        }
        else
        {
            Debug.Log("Boss moveSpeed set to: " + moveSpeed);
        }
    }

    private void Update()
    {
        if (enemy != null)
        {
            enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            if (enemy.transform.position.z < -3f)
            {
                Debug.Log("Enemy " + enemy.name + " passed z = -3 and will be destroyed.");
                GameManager gm = FindFirstObjectByType<GameManager>();
                if (gm != null)
                {
                    gm.EnemyPassedBoundary();
                }
                Destroy(enemy);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager gm = FindAnyObjectByType<GameManager>();
            if (gm != null)
            {
                // Use CurrentWave for non-boss enemies, fixed wave 4 for boss
                int waveToReport = isBoss ? 4 : gm.CurrentWave;
                gm.EnemyDestroyed(waveToReport);
            }
            if (enemy != null)
            {
                Destroy(enemy);
            }
            else
            {
                Destroy(gameObject); // Fallback if enemy reference is null
            }
        }
    }
}