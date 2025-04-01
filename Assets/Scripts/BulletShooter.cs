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
        if (crosshair == null || gun == null || mainCamera == null)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform target = bulletTarget;
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

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
                closestEnemy = enemy.transform;
            }
        }

        GameObject projectilePrefab = (gameManager != null && gameManager.HasLaser) ? gameManager.laserPrefab : bulletPrefab;
        float speed = (gameManager != null && gameManager.HasLaser) ? laserSpeed : bulletSpeed;

        if (projectilePrefab == null)
        {
            Debug.LogError("Bullet or Laser prefab is not assigned!");
            return;
        }

        Vector3 direction = (target.position - gun.position).normalized;
        Quaternion spawnRotation = (gameManager != null && gameManager.HasLaser)
            ? Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f)
            : Quaternion.identity;

        GameObject projectile = Instantiate(projectilePrefab, gun.position, spawnRotation);
        BulletScript bulletScript = projectile.GetComponent<BulletScript>();

        if (bulletScript != null)
        {
            Transform laserTarget = (gameManager != null && gameManager.HasLaser) ? closestEnemy : null;
            bulletScript.SetTarget(target.position, speed, laserTarget);
        }
    }
}
