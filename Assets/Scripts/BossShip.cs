using UnityEngine;

public class BossShip : MonoBehaviour
{
    public int health = 1000;
    private bool destroyed = false;

    public void TakeDamage(int damage)
    {
        if (destroyed) return;
        health -= damage;
        if (health <= 0)
        {
            destroyed = true;
            Destroy(gameObject);
        }
    }
}
