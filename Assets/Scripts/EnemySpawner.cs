using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject enemyPrefab;      // The enemy prefab to spawn.
    public float spawnInterval = 2f;    // Time (in seconds) between spawns.
    public float fixedZ = 150f;         // Enemies will always spawn at z = 150.

    [Header("X-Axis Spawn Settings")]
    public float xMin = -70f;           // Minimum x position.
    public float xMax = 70f;            // Maximum x position.
    public float spacingDistance = 10f; // Distance between enemy spawn points along the x-axis.

    [Header("Y-Axis Spawn Settings")]
    public float yMin = 0f;             // Minimum y position.
    public float yMax = 50f;            // Maximum y position.

    // Tracks the next x-coordinate for spawning.
    private float nextSpawnX;

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned! Please assign a valid enemy prefab in the Inspector.");
            return;
        }

        // Initialize the x position to the minimum value.
        nextSpawnX = xMin;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        // Choose a random y-coordinate between yMin and yMax.
        float randomY = Random.Range(yMin, yMax);
        // Create the spawn position using nextSpawnX for x, randomY for y, and fixedZ for z.
        Vector3 spawnPosition = new Vector3(nextSpawnX, randomY, fixedZ);

        // Instantiate the enemy and parent it under this GameObject (EnemyManager).
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
        Debug.Log("Spawned enemy at: " + spawnPosition);

        // Increment nextSpawnX by the spacing distance.
        nextSpawnX += spacingDistance;
        // If we've exceeded xMax, loop back to xMin.
        if (nextSpawnX > xMax)
        {
            nextSpawnX = xMin;
        }
    }

    // Optional: Visualize the x-axis spawn range in the Scene view.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Draw a line between xMin and xMax at the minimum y position, with z fixed.
        Vector3 startPos = new Vector3(xMin, yMin, fixedZ);
        Vector3 endPos = new Vector3(xMax, yMin, fixedZ);
        Gizmos.DrawLine(startPos, endPos);
        // Also draw spheres at the endpoints.
        Gizmos.DrawWireSphere(startPos, 1f);
        Gizmos.DrawWireSphere(endPos, 1f);
    }
}
