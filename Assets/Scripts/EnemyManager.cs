using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public int health = 5; 
    public float moveSpeed = 30f;
    public Vector3 moveDirection = Vector3.back;
    public bool isBoss = false; 
    public int wave = 1;

    private GameManager gameManager; 
    private const float ZBoundary = -3f;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        if (enemy == null && transform.parent != null)
        {
            enemy = transform.parent.gameObject;
        }

        if (isBoss)
        {
            health = 100; 
        }
        else
        {
            moveSpeed = Random.Range(5f, 20f); 
        }

        Debug.Log($"Enemy initialized: Name = {gameObject.name}, Position = {transform.position}, IsBoss = {isBoss}, Health = {health}, Enemy Ref = {(enemy != null ? enemy.name : "null")}");
    }

    private void Update()
    {
        if (enemy == null) return;

        enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (enemy.transform.position.z < ZBoundary)
        {
            if (!isBoss)
            {
                Debug.Log($"Normal enemy passed boundary: Name = {enemy.name}, Position = {enemy.transform.position}, Wave = {wave}");
                gameManager?.EnemyPassedBoundary(); 
                Destroy(enemy);
            }
            else if (isBoss)
            {
                Debug.Log($"Boss passed boundary: Name = {enemy.name}, Position = {enemy.transform.position}, calling GameOver");
                gameManager?.GameOver(); 
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