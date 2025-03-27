using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;  // Assign the Bullet prefab
    public Transform leftGun, rightGun;  // Assign both gun positions
    public Transform bulletTarget;  // Assign the BulletTarget (empty GameObject in front) as fallback
    public RectTransform crosshair;  // Assign the crosshair UI element (RectTransform)
    public float bulletSpeed = 20f;
    public float crosshairRange = 100f;  // Max 2D screen distance for auto-aim (in pixels)
    private Camera mainCamera;  // Reference to the main camera

    void Start()
    {
        // Get the main camera reference
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        { // Fire bullets on click
            Shoot(leftGun);
            Shoot(rightGun);
        }
    }

    void Shoot(Transform gun)
    {
        // Find the closest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Tag your enemies with "Enemy"
        Transform target = bulletTarget; // Default to fixed target
        float closestDistance = float.MaxValue;
        Vector2 crosshairScreenPos = crosshair.position; // Crosshair's 2D screen position

        foreach (GameObject enemy in enemies)
        {
            // Get 3D distance from gun to enemy (for bullet travel)
            float distance3D = Vector3.Distance(gun.position, enemy.transform.position);

            // Get enemy's 2D screen position
            Vector3 enemyScreenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);
            float distance2D = Vector2.Distance(crosshairScreenPos, new Vector2(enemyScreenPos.x, enemyScreenPos.y));

            // Check if this enemy is the closest in 3D and within crosshair range in 2D
            if (distance3D < closestDistance && distance2D <= crosshairRange)
            {
                closestDistance = distance3D;
                target = enemy.transform;
            }
        }

        // Instantiate and direct the bullet
        GameObject bullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target.position, bulletSpeed);
        }
    }
}