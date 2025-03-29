using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;         // The main enemy object (parent).
    public int health = 5;
    public float moveSpeed = 30f;      // Fixed speed for bosses.
    public Vector3 moveDirection = Vector3.forward;

    // For normal enemies, set isBoss to false.
    public bool isBoss = true;
    // The 'wave' field is not used for scoring; GameManager.CurrentWave is used for non-boss enemies.
    public int wave = 1;

    private void Start()
    {
        if (enemy == null && transform.parent != null)
        {
            enemy = transform.parent.gameObject;
        }

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
            enemy.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            if (enemy.transform.position.z < -3f)
            {
                Debug.Log("Enemy " + enemy.name + " passed z = -3 and will be destroyed.");
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.EnemyPassedBoundary();
                }
                Destroy(enemy);
            }
        }
    }
}
