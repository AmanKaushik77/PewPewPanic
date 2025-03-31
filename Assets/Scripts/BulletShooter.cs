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
    private AudioManager audioManager;

    void Start()
    {
        mainCamera = Camera.main;
        gameManager = FindFirstObjectByType<GameManager>();
        audioManager = FindFirstObjectByType<AudioManager>();

        if (gameManager == null)
            Debug.LogError("GameManager not found!");
        if (audioManager == null)
            Debug.LogError("AudioManager not found!");
    }

    void Update()
    {
        // If the crosshair or audioManager are missing/destroyed, no shooting.
        if (crosshair == null || audioManager == null)
            return;

        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0f)
        {
            audioManager.PlayShootSound();
            Shoot(leftGun);
            Shoot(rightGun);
        }
    }

    void Shoot(Transform gun)
    {
        // Safeguard in case the crosshair or gun got destroyed/unassigned
        if (crosshair == null || gun == null || mainCamera == null)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform target = bulletTarget; // Default target if no enemy is found
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null; // Only used for lasers

        // Convert crosshair's screen position to a Vector2
        Vector2 crosshairScreenPos = crosshair.position;

        foreach (GameObject enemy in enemies)
        {
            // Distance in the 3D world
            float distance3D = Vector3.Distance(gun.position, enemy.transform.position);

            // Distance in the 2D screen space, relative to crosshair
            Vector3 enemyScreenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);
            float distance2D = Vector2.Distance(crosshairScreenPos, new Vector2(enemyScreenPos.x, enemyScreenPos.y));

            // Pick the closest enemy within crosshair range
            if (distance3D < closestDistance && distance2D <= crosshairRange)
            {
                closestDistance = distance3D;
                target = enemy.transform;
                closestEnemy = enemy.transform; // Store for laser tracking
            }
        }

        GameObject projectilePrefab = (gameManager != null && gameManager.HasLaser) ? gameManager.laserPrefab : bulletPrefab;
        float speed = (gameManager != null && gameManager.HasLaser) ? laserSpeed : bulletSpeed;

        if (projectilePrefab == null)
        {
            Debug.LogError("Bullet or Laser prefab is not assigned!");
            return;
        }

        // Aim bullet/laser at the chosen target position
        Vector3 direction = (target.position - gun.position).normalized;
        Quaternion spawnRotation = (gameManager != null && gameManager.HasLaser)
            ? Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f) // Lasers with original rotation
            : Quaternion.identity; // Bullets unrotated

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, gun.position, spawnRotation);
        BulletScript bulletScript = projectile.GetComponent<BulletScript>();

        if (bulletScript != null)
        {
            // Pass enemy Transform only for lasers, null for bullets
            Transform laserTarget = (gameManager != null && gameManager.HasLaser) ? closestEnemy : null;
            bulletScript.SetTarget(target.position, speed, laserTarget);
        }
    }
}