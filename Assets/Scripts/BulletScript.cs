using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;

    public void SetTarget(Vector3 target, float bulletSpeed)
    {
        targetPosition = target;
        speed = bulletSpeed;
    }

    void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Rotate the bullet to face the target
            transform.LookAt(targetPosition);

            // Destroy bullet when it reaches the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
