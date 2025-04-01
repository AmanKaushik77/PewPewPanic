using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings (Normal Enemies)")]
    private int[] easyWaveEnemyCounts = new int[] { 7, 7, 7 };   // 7 enemies per wave for Easy
    private int[] mediumWaveEnemyCounts = new int[] { 9, 9, 9 }; // 9 enemies per wave for Medium
    private int[] hardWaveEnemyCounts = new int[] { 10, 10, 10 }; // 10 enemies per wave for Hard
    public float spawnInterval = 1f; // Base value, adjusted to Hard's 0.45f
    public float timeBetweenWaves = 3f; // Base value, adjusted to Hard's 1.5f

    [Header("Spawn Area Settings")]
    public float xMin = -70f;
    public float xMax = 70f;
    public float yMin = 0f;
    public float yMax = 50f;
    public float fixedZ = 150f;

    [Header("Post-Wave Boss & Fast Enemy Settings")]
    public Vector3 bossShipSpawnPosition = new Vector3(0f, 20f, 175f);
    public float fastEnemySpawnInterval = 5f; // Base value, adjusted to Hard's 2.25f

    [Header("References (Prefabs)")]
    public GameObject enemyPrefab;
    public GameObject fastEnemyShipPrefab;
    public GameObject bossShipPrefab;

    [Header("Power-Up UI")]
    public GameObject powerUpPanel;
    private bool powerUpSelected = false;

    private GameManager gameManager;
    private System.Collections.Generic.List<GameObject> activeEnemies = new System.Collections.Generic.List<GameObject>();
    private int[] adjustedWaveEnemyCounts;
    private float adjustedSpawnInterval;
    private float adjustedFastEnemyInterval;
    private float adjustedTimeBetweenWaves;

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
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }

        AdjustDifficultySettings();
        powerUpPanel.SetActive(false);
        StartCoroutine(SpawnWaves());
    }

    void AdjustDifficultySettings()
    {
        int difficulty = DifficultyManager.CurrentDifficulty;
        // Set enemy counts based on difficulty
        switch (difficulty)
        {
            case 1: // Easy
                adjustedWaveEnemyCounts = easyWaveEnemyCounts; // 7, 7, 7
                break;
            case 2: // Medium
                adjustedWaveEnemyCounts = mediumWaveEnemyCounts; // 9, 9, 9
                break;
            case 3: // Hard
                adjustedWaveEnemyCounts = hardWaveEnemyCounts; // 10, 10, 10
                break;
            default:
                adjustedWaveEnemyCounts = mediumWaveEnemyCounts; // Default to Medium if difficulty is invalid
                break;
        }

        // Use Hard difficulty timing for all levels
        adjustedSpawnInterval = spawnInterval * 0.45f; // 0.45f
        adjustedFastEnemyInterval = fastEnemySpawnInterval * 0.45f; // 2.25f
        adjustedTimeBetweenWaves = timeBetweenWaves * 0.5f; // 1.5f

        Debug.Log($"Difficulty set to {difficulty}: Enemies per wave: {string.Join(", ", adjustedWaveEnemyCounts)}, Spawn Interval: {adjustedSpawnInterval}, Fast Enemy Interval: {adjustedFastEnemyInterval}, Time Between Waves: {adjustedTimeBetweenWaves}");
    }

    IEnumerator SpawnWaves()
    {
        for (int waveIndex = 0; waveIndex < adjustedWaveEnemyCounts.Length; waveIndex++)
        {
            int enemyCount = adjustedWaveEnemyCounts[waveIndex];
            Debug.Log("Starting Wave " + (waveIndex + 1) + " with " + enemyCount + " enemy(ies).");

            for (int i = 0; i < enemyCount; i++)
            {
                float randomX = Random.Range(xMin, xMax);
                float randomY = Random.Range(yMin, yMax);
                Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
                EnemyManager em = enemy.GetComponentInChildren<EnemyManager>();
                if (em != null)
                {
                    em.enemy = enemy;
                    em.isBoss = false;
                }
                activeEnemies.Add(enemy);
                Debug.Log($"Spawned enemy {i + 1}/{enemyCount} in Wave {waveIndex + 1} at: {spawnPosition}, Time: {Time.time}");
                yield return new WaitForSeconds(adjustedSpawnInterval);
            }

            Debug.Log("Completed spawning Wave " + (waveIndex + 1) + ". Waiting for all enemies to be destroyed...");
            while (activeEnemies.Count > 0)
            {
                activeEnemies.RemoveAll(item => item == null);
                yield return null;
            }
            Debug.Log("Wave " + (waveIndex + 1) + " cleared.");

            if (waveIndex < adjustedWaveEnemyCounts.Length - 1)
            {
                gameManager.SetWave(waveIndex + 2);
                powerUpSelected = false;
                powerUpPanel.SetActive(true);
                Time.timeScale = 0f;
                yield return new WaitUntil(() => powerUpSelected);
                Time.timeScale = 1f;
                powerUpPanel.SetActive(false);
                yield return new WaitForSeconds(adjustedTimeBetweenWaves);
            }
        }
        Debug.Log("All normal waves completed. Spawning BossShip and FastEnemyShips...");
        SpawnBossObjects();
    }

    void SpawnBossObjects()
    {
        GameObject boss = Instantiate(bossShipPrefab, bossShipSpawnPosition, Quaternion.Euler(0, 180, 0));
        EnemyManager bossEm = boss.GetComponentInChildren<EnemyManager>();
        if (bossEm != null)
        {
            bossEm.enemy = boss;
            bossEm.isBoss = true;
        }
        Debug.Log("BossShip spawned at: " + bossShipSpawnPosition);
        StartCoroutine(SpawnFastEnemyShips(boss));
    }

    IEnumerator SpawnFastEnemyShips(GameObject boss)
    {
        while (boss != null)
        {
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);
            Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);
            GameObject fastEnemy = Instantiate(fastEnemyShipPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
            EnemyManager fastEm = fastEnemy.GetComponentInChildren<EnemyManager>();
            if (fastEm != null)
            {
                fastEm.enemy = fastEnemy;
                fastEm.isBoss = false;
            }
            Debug.Log("FastEnemyShip spawned at: " + spawnPosition);
            yield return new WaitForSeconds(adjustedFastEnemyInterval);
        }
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