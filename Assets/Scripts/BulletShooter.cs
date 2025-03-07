using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;  // Assign the Bullet prefab
    public Transform leftGun, rightGun;  // Assign both gun positions
    public Transform bulletTarget;  // Assign the BulletTarget (empty GameObject in front)
    public float bulletSpeed = 20f;

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
        GameObject bullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(bulletTarget.position, bulletSpeed);
        }
    }
}
