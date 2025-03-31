using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings (Normal Enemies)")]
    public int[] waveEnemyCounts = new int[] { 1, 2, 3 };
    public float spawnInterval = 1f;
    public float timeBetweenWaves = 10f;

    [Header("Spawn Area Settings")]
    public float xMin = -70f;
    public float xMax = 70f;
    public float yMin = 0f;
    public float yMax = 50f;
    public float fixedZ = 150f;

    [Header("Post-Wave Boss & Fast Enemy Settings")]
    public Vector3 bossShipSpawnPosition = new Vector3(0f, 20f, 175f);
    public float fastEnemySpawnInterval = 5f;

    [Header("References (Prefabs)")]
    public GameObject enemyPrefab;
    public GameObject fastEnemyShipPrefab;
    public GameObject bossShipPrefab;

    [Header("Power-Up UI")]
    public GameObject powerUpPanel;
    private bool powerUpSelected = false;

    private GameManager gameManager;
    private System.Collections.Generic.List<GameObject> activeEnemies = new System.Collections.Generic.List<GameObject>();

    private void Start()
    {
        if (enemyPrefab == null || bossShipPrefab == null || fastEnemyShipPrefab == null)
        {
            Debug.LogError("One or more prefabs are not assigned in the WaveManager!");
            return;
        }
        if (powerUpPanel == null)
        {
            Debug.LogError("PowerUp Panel is not assigned in the WaveManager!");
            return;
        }
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }
        powerUpPanel.SetActive(false);
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int waveIndex = 0; waveIndex < waveEnemyCounts.Length; waveIndex++)
        {
            int enemyCount = waveEnemyCounts[waveIndex];
            Debug.Log("Starting Wave " + (waveIndex + 1) + " with " + enemyCount + " enemy(ies).");

            for (int i = 0; i < enemyCount; i++)
            {
                float randomX = Random.Range(xMin, xMax);
                float randomY = Random.Range(yMin, yMax);
                Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
                activeEnemies.Add(enemy);
                Debug.Log("Spawned normal enemy at: " + spawnPosition);
                yield return new WaitForSeconds(spawnInterval);
            }

            Debug.Log("Completed spawning Wave " + (waveIndex + 1) + ". Waiting for all enemies to be destroyed...");
            while (activeEnemies.Count > 0)
            {
                activeEnemies.RemoveAll(item => item == null);
                yield return null;
            }
            Debug.Log("Wave " + (waveIndex + 1) + " cleared.");

            if (waveIndex < waveEnemyCounts.Length - 1)
            {
                gameManager.SetWave(waveIndex + 2);
                powerUpSelected = false;
                powerUpPanel.SetActive(true);
                Time.timeScale = 0f;
                yield return new WaitUntil(() => powerUpSelected);
                Time.timeScale = 1f;
                powerUpPanel.SetActive(false);
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        Debug.Log("All normal waves completed. Spawning BossShip and FastEnemyShips...");
        SpawnBossObjects();
    }

    void SpawnBossObjects()
    {
        GameObject boss = Instantiate(bossShipPrefab, bossShipSpawnPosition, Quaternion.Euler(0, 180, 0));
        Debug.Log("BossShip spawned at: " + bossShipSpawnPosition);
        StartCoroutine(SpawnFastEnemyShips(boss));
    }

    IEnumerator SpawnFastEnemyShips(GameObject boss)
{
    while (boss != null)
    {
        // 1. Check Boss z-position
        if (boss.transform.position.z == -1f)
        {
            Debug.Log("Boss crossed z = -2.5. Pausing the game and showing Game Over screen.");
            
            // 2. Pause the game
            Time.timeScale = 0f;

            // 3. Display Game Over UI / Logic
            gameManager.GameOver(); // Assuming this method handles the Game Over screen

            // 4. End this coroutine
            yield break;
        }

        // Regular spawn logic (unchanged)
        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);
        Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);

        Instantiate(fastEnemyShipPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
        Debug.Log("FastEnemyShip spawned at: " + spawnPosition);

        yield return new WaitForSeconds(fastEnemySpawnInterval);
    }

    // If the Boss is destroyed (null), the loop will end normally
    Debug.Log("Boss has been destroyed. Notifying GameManager.");
    gameManager.BossDestroyed();
}



    public void SelectLaserPowerUp()
    {
        gameManager.ActivateLaserPowerUp();
        Debug.Log("Laser Power-Up Selected");
        powerUpSelected = true;
    }

    public void SelectSpeedBuffPowerUp()
    {
        gameManager.ActivateSpeedBuff();
        Debug.Log("Speed Buff Power-Up Selected");
        powerUpSelected = true;
    }
}