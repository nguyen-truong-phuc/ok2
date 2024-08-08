using UnityEngine;

public class RotatingTower : MonoBehaviour
{
    public float range = 5f; // Khoảng cách tấn công của tháp
    public float fireRate = 1f; // Tốc độ bắn
    public GameObject bulletPrefab; // Prefab của đạn
    public Transform firePoint; // Vị trí bắn
    public float rotationSpeed = 10f; // Tốc độ xoay

    private float fireCountdown = 0f; // Thời gian chờ giữa các phát bắn
    private Transform target; // Mục tiêu của tháp

    private void Update()
    {
        UpdateTarget();

        if (target == null)
            return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Seek(target);
        }
    }
}
