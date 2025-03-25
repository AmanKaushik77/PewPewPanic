using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public GameObject enemy;           // The main enemy object (parent)
    public int health = 5;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter fired on " + gameObject.name + 
                  " because of: " + other.gameObject.name);

        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected!");
            health--;
            Debug.Log("Health is now: " + health);

            // Destroy the bullet
            Destroy(other.gameObject);

            // Destroy the enemy if health <= 0
            if (health <= 0)
            {
                Debug.Log("Destroying enemy object: " + enemy.name);
                Destroy(enemy);
            }
        }
    }
}
