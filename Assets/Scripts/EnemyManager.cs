using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;           // The main enemy object (parent)
    public int health = 5;
    public float moveSpeed = 30f;        // For bosses, we want a fixed speed of 30.
    public Vector3 moveDirection = Vector3.forward;
    
    // Set this flag on non-boss enemies if needed.
    public bool isBoss = true;           

    private void Start()
    {
        // If enemy not assigned, assume the parent is the enemy.
        if (enemy == null && transform.parent != null)
        {
            enemy = transform.parent.gameObject;
        }
        
        // For non-boss enemies, you might use random speed.
        if (!isBoss)
        {
            moveSpeed = Random.Range(5f, 20f);
            Debug.Log("Assigned random moveSpeed: " + moveSpeed);
        }
        else
        {
            Debug.Log("Boss moveSpeed set to: " + moveSpeed);
        }
    }

    private void Update()
    {
        if (enemy != null)
        {
            // Move the enemy.
            enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Check if the enemy has passed z = -3.
            if (enemy.transform.position.z < -3f)
            {
                Debug.Log("Enemy " + enemy.name + " passed z = -3 and will be destroyed.");
                Destroy(enemy);
            }
        }
    }
}
