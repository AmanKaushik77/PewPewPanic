using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    public float moveSpeed = 50f;
    public Vector3 moveDirection = Vector3.forward; // Moves enemy to the right
    public int health = 25; // Initial health of the enemy

    private void Update()
    {
        if (enemy != null)
        {
            enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(enemy);
        }
    }
}