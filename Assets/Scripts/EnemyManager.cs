using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public int health = 5;
    public float moveSpeed = 30f;
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
        }
    }

    private void Update()
    {
        if (enemy == null) return;
        enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        if (enemy.transform.position.z < -3f)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (!isBoss)
            {
                if (gm != null) gm.EnemyPassedBoundary();
                Destroy(enemy);
            }
            else
            {
                if (gm != null) gm.GameOver();
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
                int waveToReport = isBoss ? 4 : gm.CurrentWave;
                gm.EnemyDestroyed(waveToReport);
            }
            if (enemy != null) Destroy(enemy);
            else Destroy(gameObject);
        }
    }
}
