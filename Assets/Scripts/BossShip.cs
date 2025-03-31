using UnityEngine;

public class BossShip : MonoBehaviour
{
    public int health = 100;
    private bool destroyed = false;

    public void TakeDamage(int damage)
    {
        if (destroyed) return;
        health -= damage;
        if (health <= 0)
        {
            destroyed = true;
            // This will make bossInstance == null from WaveManager's perspective
            Destroy(gameObject);
        }
    }
}
