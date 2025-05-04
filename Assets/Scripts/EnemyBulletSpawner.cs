using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    void Start()
    {
        InvokeRepeating(nameof(Shoot), 0, 1);
    }
    void Shoot()
    {
        if (bulletPrefab != null)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
