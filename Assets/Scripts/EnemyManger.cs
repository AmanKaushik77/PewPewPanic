using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject bullet;
    public float moveSpeed = 50f;
    public Vector3 moveDirection = Vector3.forward; // Moves enemy to the right
    public int health = 5; // Initial health of the enemy

    private void Update()
    {
        if (enemy != null)
        {
            enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);
        if (other.CompareTag("Bullet")) // Check tag instead of name
        {
            Debug.Log("Bullet hit! Health before hit: " + health);
            TakeDamage();
            Debug.Log("Health after hit: " + health);
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Debug.Log("Enemy destroyed!");
            Destroy(enemy);
        }
    }
}
