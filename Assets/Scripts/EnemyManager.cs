using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;           // The main enemy object (parent)
    public int health = 5;
    public float moveSpeed = 5f;
    public Vector3 moveDirection = Vector3.forward;

    private void Start()
    {
        // If enemy not assigned, assume the parent is the enemy.
        if (enemy == null && transform.parent != null)
        {
            enemy = transform.parent.gameObject;
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
