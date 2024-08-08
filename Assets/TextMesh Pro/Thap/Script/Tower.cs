using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 5f; // Khoảng cách tấn công của tháp
    public float fireRate = 1f; // Tốc độ bắn
    public GameObject bulletPrefab; // Prefab của đạn
    public Transform firePoint; // Vị trí bắn

    private float fireCountdown = 0f; // Thời gian chờ giữa các phát bắn

    private void Update()
    {
        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    private void Shoot()
    {
        // Tìm enemy gần nhất
        GameObject targetEnemy = FindNearestEnemy();
        if (targetEnemy != null)
        {
            // Tạo đạn và bắn
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Seek(targetEnemy.transform);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
