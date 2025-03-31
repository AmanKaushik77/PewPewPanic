using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform leftGun, rightGun;
    public Transform bulletTarget;
    public RectTransform crosshair;
    public float bulletSpeed = 20f;
    public float laserSpeed = 50f;
    public float crosshairRange = 200f;

    private Camera mainCamera;
    private GameManager gameManager;

    void Start()
    {
        mainCamera = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null) Debug.LogError("GameManager not found!");
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(leftGun);
            Shoot(rightGun);
        }
    }

    void Shoot(Transform gun)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform target = bulletTarget;
        float closestDistance = float.MaxValue;
        Vector2 crosshairScreenPos = crosshair.position;

        foreach (GameObject enemy in enemies)
        {
            float distance3D = Vector3.Distance(gun.position, enemy.transform.position);
            Vector3 enemyScreenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);
            float distance2D = Vector2.Distance(crosshairScreenPos, new Vector2(enemyScreenPos.x, enemyScreenPos.y));

            if (distance3D < closestDistance && distance2D <= crosshairRange)
            {
                closestDistance = distance3D;
                target = enemy.transform;
            }
        }

        GameObject projectilePrefab = (gameManager != null && gameManager.HasLaser) ? gameManager.laserPrefab : bulletPrefab;
        float speed = (gameManager != null && gameManager.HasLaser) ? laserSpeed : bulletSpeed;

        if (projectilePrefab == null)
        {
            Debug.LogError("Bullet or Laser prefab is not assigned!");
            return;
        }

        // Calculate direction to target
        Vector3 direction = (target.position - gun.position).normalized;

        // For laser: Align local Z (long axis) with target direction, keeping prefab's X = 90
        Quaternion spawnRotation = (gameManager != null && gameManager.HasLaser) ?
            Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f) : // Laser: Point Z at target
            Quaternion.identity; // Bullet: Default orientation

        GameObject projectile = Instantiate(projectilePrefab, gun.position, spawnRotation);
        BulletScript bulletScript = projectile.GetComponent<BulletScript>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target.position, speed);
        }
    }
}