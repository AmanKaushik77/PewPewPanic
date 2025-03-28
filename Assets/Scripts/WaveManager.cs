using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings (Normal Enemies)")]
    // For testing: wave sizes are set to 1, 2, and 3 enemies.
    public int[] waveEnemyCounts = new int[] { 1, 2, 3 };
    public float spawnInterval = 1f;      // Time between enemy spawns within a wave.
    public float timeBetweenWaves = 10f;  // Wait time between waves.

    [Header("Spawn Area Settings (for Normal Enemies & FastEnemyShips)")]
    public float xMin = -70f;             // Minimum x position.
    public float xMax = 70f;              // Maximum x position.
    public float yMin = 0f;               // Minimum y position.
    public float yMax = 50f;              // Maximum y position.
    public float fixedZ = 150f;           // All enemies spawn at z = 150.

    [Header("Post-Wave Boss & Fast Enemy Settings")]
    // BossShip spawns at a fixed position.
    public Vector3 bossShipSpawnPosition = new Vector3(0f, 20f, 175f);
    // Number of FastEnemyShips to spawn.
    public int fastEnemyShipCount = 7;
    // Delay between each FastEnemyShip spawn.
    public float fastEnemySpawnInterval = 5f;

    [Header("References (Prefabs)")]
    public GameObject enemyPrefab;            // "Enemy" prefab (normal enemy).
    public GameObject fastEnemyShipPrefab;      // "FastEnemyShip" prefab.
    public GameObject bossShipPrefab;           // "BossShip" prefab.

    // List to track spawned normal enemies.
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in the WaveManager!");
            return;
        }
        if (bossShipPrefab == null)
        {
            Debug.LogError("BossShip prefab is not assigned in the WaveManager!");
            return;
        }
        if (fastEnemyShipPrefab == null)
        {
            Debug.LogError("FastEnemyShip prefab is not assigned in the WaveManager!");
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        // Loop through each wave as defined in waveEnemyCounts.
        for (int waveIndex = 0; waveIndex < waveEnemyCounts.Length; waveIndex++)
        {
            int enemyCount = waveEnemyCounts[waveIndex];
            Debug.Log("Starting Wave " + (waveIndex + 1) + " with " + enemyCount + " enemy(ies).");

            // Spawn the designated number of normal enemies.
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

            // Wait until all normal enemies from this wave are destroyed.
            while (activeEnemies.Count > 0)
            {
                activeEnemies.RemoveAll(item => item == null);
                yield return null;
            }

            Debug.Log("Wave " + (waveIndex + 1) + " cleared.");

            // Wait before starting the next wave (if not the last one).
            if (waveIndex < waveEnemyCounts.Length - 1)
            {
                Debug.Log("Waiting for next wave...");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        Debug.Log("All normal waves completed. Spawning BossShip and FastEnemyShips...");
        SpawnBossObjects();
    }

    void SpawnBossObjects()
    {
        // Spawn the BossShip at the fixed coordinates.
        GameObject boss = Instantiate(bossShipPrefab, bossShipSpawnPosition, Quaternion.identity);
        Debug.Log("BossShip spawned at: " + bossShipSpawnPosition);

        // Start a coroutine to spawn FastEnemyShips one by one.
        StartCoroutine(SpawnFastEnemyShips());
    }

    IEnumerator SpawnFastEnemyShips()
    {
        for (int i = 0; i < fastEnemyShipCount; i++)
        {
            // Spawn using the same area settings as normal enemies.
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);
            Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);

            GameObject fastEnemy = Instantiate(fastEnemyShipPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("FastEnemyShip spawned at: " + spawnPosition);

            yield return new WaitForSeconds(fastEnemySpawnInterval);
        }
    }
}
