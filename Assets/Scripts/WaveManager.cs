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
    public int fastEnemyShipCount = 7;
    public float fastEnemySpawnInterval = 5f;

    [Header("References (Prefabs)")]
    public GameObject enemyPrefab;
    public GameObject fastEnemyShipPrefab;
    public GameObject bossShipPrefab;

    [Header("Power-Up UI")]
    public GameObject powerUpPanel; // Assign this in the Inspector
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

        powerUpPanel.SetActive(false); // Hide panel at start
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

                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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

                // Show power-up selection UI
                powerUpSelected = false;
                powerUpPanel.SetActive(true);
                Time.timeScale = 0f; // Pause game while selecting

                yield return new WaitUntil(() => powerUpSelected);

                Time.timeScale = 1f; // Resume game
                powerUpPanel.SetActive(false);
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        Debug.Log("All normal waves completed. Spawning BossShip and FastEnemyShips...");
        SpawnBossObjects();
    }

    void SpawnBossObjects()
    {
        Instantiate(bossShipPrefab, bossShipSpawnPosition, Quaternion.identity);
        Debug.Log("BossShip spawned at: " + bossShipSpawnPosition);
        StartCoroutine(SpawnFastEnemyShips());
    }

    IEnumerator SpawnFastEnemyShips()
    {
        for (int i = 0; i < fastEnemyShipCount; i++)
        {
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);
            Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);

            Instantiate(fastEnemyShipPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("FastEnemyShip spawned at: " + spawnPosition);

            yield return new WaitForSeconds(fastEnemySpawnInterval);
        }
    }

    // Power-up selection methods
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