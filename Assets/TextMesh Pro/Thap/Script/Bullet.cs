using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Tốc độ đạn
    public float damage = 10f; // Sát thương của đạn

    private Transform target; // Mục tiêu của đạn

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }


    private void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            GameManager.instance.AddScore(100); // Cộng điểm khi tiêu diệt enemy
            Debug.Log("Enemy chết, cộng 100 điểm"); // Thêm thông báo debug
        }
        Destroy(gameObject);
    }


}
