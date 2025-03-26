using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int[] waveEnemyCounts = new int[] { 5, 10, 15 };
    public float spawnInterval = 1f;      // Time between enemy spawns within a wave.
    public float timeBetweenWaves = 10f;   // Wait time between waves.

    [Header("Spawn Area Settings")]
    public float xMin = -70f;             // Minimum x position.
    public float xMax = 70f;              // Maximum x position.
    public float yMin = 0f;               // Minimum y position.
    public float yMax = 50f;              // Maximum y position.
    public float fixedZ = 150f;           // All enemies spawn at z = 150.

    [Header("References")]
    public GameObject enemyPrefab;        // Enemy prefab to spawn.

    // List to track spawned enemies.
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in the WaveManager!");
            return;
        }
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        // Loop through each wave as defined in waveEnemyCounts.
        for (int waveIndex = 0; waveIndex < waveEnemyCounts.Length; waveIndex++)
        {
            Debug.Log("Starting Wave " + (waveIndex + 1));
            int enemyCount = waveEnemyCounts[waveIndex];

            // Spawn the designated number of enemies for this wave.
            for (int i = 0; i < enemyCount; i++)
            {
                float randomX = Random.Range(xMin, xMax);
                float randomY = Random.Range(yMin, yMax);
                Vector3 spawnPosition = new Vector3(randomX, randomY, fixedZ);

                // Instantiate enemy and add it to the active enemies list.
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                activeEnemies.Add(enemy);
                Debug.Log("Spawned enemy at: " + spawnPosition);

                yield return new WaitForSeconds(spawnInterval);
            }

            Debug.Log("Completed spawning Wave " + (waveIndex + 1) + ". Waiting for all enemies to be destroyed...");

            // Wait until all enemies from this wave are destroyed.
            while (activeEnemies.Count > 0)
            {
                // Remove any null entries (destroyed enemies) from the list.
                activeEnemies.RemoveAll(item => item == null);
                yield return null;
            }

            Debug.Log("Wave " + (waveIndex + 1) + " cleared.");
            // Wait additional time between waves (if not the last wave).
            if (waveIndex < waveEnemyCounts.Length - 1)
            {
                Debug.Log("Waiting for next wave...");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        Debug.Log("All waves completed.");
    }
}
