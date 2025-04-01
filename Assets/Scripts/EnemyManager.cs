using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public int health = 5; // Default health for normal enemies
    public float moveSpeed = 30f;
    public Vector3 moveDirection = Vector3.back;
    public bool isBoss = false; // Default to false for normal enemies
    public int wave = 1;

    private GameManager gameManager; // Cached reference to avoid repeated Find calls
    private const float ZBoundary = -3f;

    private void Start()
    {
        // Cache GameManager once at start
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        // Set enemy reference if not assigned
        if (enemy == null && transform.parent != null)
        {
            enemy = transform.parent.gameObject;
        }

        // Set health and speed based on whether it's a boss
        if (isBoss)
        {
            health = 100; // Boss health set to 1000
        }
        else
        {
            moveSpeed = Random.Range(5f, 20f); // Random speed for normal enemies
        }

        Debug.Log($"Enemy initialized: Name = {gameObject.name}, Position = {transform.position}, IsBoss = {isBoss}, Health = {health}, Enemy Ref = {(enemy != null ? enemy.name : "null")}");
    }

    private void Update()
    {
        if (enemy == null) return;

        // Move enemy
        enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (enemy.transform.position.z < ZBoundary)
        {
            if (!isBoss)
            {
                Debug.Log($"Normal enemy passed boundary: Name = {enemy.name}, Position = {enemy.transform.position}, Wave = {wave}");
                gameManager?.EnemyPassedBoundary(); // Trigger life reduction
                Destroy(enemy);
            }
            else if (isBoss)
            {
                Debug.Log($"Boss passed boundary: Name = {enemy.name}, Position = {enemy.transform.position}, calling GameOver");
                gameManager?.GameOver(); // End game if boss passes
                Destroy(enemy);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (gameManager != null)
            {
                int waveToReport = isBoss ? 4 : gameManager.CurrentWave;
                gameManager.EnemyDestroyed(waveToReport);
                Debug.Log($"Enemy destroyed: Name = {gameObject.name}, Wave = {waveToReport}");
            }
            if (enemy != null) Destroy(enemy);
            else Destroy(gameObject);
        }
    }
}