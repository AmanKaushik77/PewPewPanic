using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public GameObject enemy; 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter fired on " + gameObject.name + " because of: " + other.gameObject.name);

        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit detected!");
            EnemyManager manager = enemy.GetComponent<EnemyManager>();
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (manager != null)
            {
                int damage = (gm != null && gm.HasLaser) ? 5 : 1; 
                manager.TakeDamage(damage);
                Debug.Log($"Damage {damage} delegated to EnemyManager");
            }
            Destroy(other.gameObject);
        }
    }
}