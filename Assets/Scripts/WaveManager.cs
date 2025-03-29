using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings (Normal Enemies)")]
    public int[] waveEnemyCounts = new int[] { 1, 2, 3 };  // For testing: 1 enemy in wave 1, 2 in wave 2, 3 in wave 3.
    public float spawnInterval = 1f;       // Time between enemy spawns within a wave.
    public float timeBetweenWaves = 10f;   // Wait time between waves.

    [Header("Spawn Area Settings")]
    public float xMin = -70f;
    public float xMax = 70f;
    public float yMin = 0f;
    public float yMax = 50f;
    public float fixedZ = 150f;            // All enemies spawn at z = 150.

    [Header("Post-Wave Boss & Fast Enemy Settings")]
    public Vector3 bossShipSpawnPosition = new Vector3(0f, 20f, 175f);
    public int fastEnemyShipCount = 7;
    public float fastEnemySpawnInterval = 5f;

    [Header("References (Prefabs)")]
    public GameObject enemyPrefab;         // Normal enemy prefab.
    public GameObject fastEnemyShipPrefab;   // Fast enemy prefab.
    public GameObject bossShipPrefab;        // Boss enemy prefab.

    // List to track spawned normal enemies.
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        if (enemyPrefab == null || bossShipPrefab == null || fastEnemyShipPrefab == null)
        {
            Debug.LogError("One or more prefabs are not assigned in the WaveManager!");
            return;
        }
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        // The GameManager should already display "Wave: 1" at start.
        GameManager gm = FindObjectOfType<GameManager>();

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

            // Wait until all enemies from this wave are destroyed.
            while (activeEnemies.Count > 0)
            {
                activeEnemies.RemoveAll(item => item == null);
                yield return null;
            }

            Debug.Log("Wave " + (waveIndex + 1) + " cleared.");

            // If not the last wave, update the GameManager for the next wave.
            if (waveIndex < waveEnemyCounts.Length - 1)
            {
                if (gm != null)
                {
                    gm.SetWave(waveIndex + 2);  // For example, after wave 1, set wave to 2.
                }
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
}
