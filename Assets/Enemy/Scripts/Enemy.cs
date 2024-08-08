using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Máu của enemy
    public float speed = 1f; // Tốc độ di chuyển của enemy
    public List<Transform> waypoints; // Danh sách các waypoint
    public int goldReward = 100; // Vàng thưởng khi enemy chết

    private int currentWaypointIndex = 0; // Chỉ số của waypoint hiện tại

    private void Start()
    {
        if (waypoints.Count > 0)
        {
            // Đặt vị trí bắt đầu tại waypoint đầu tiên
            transform.position = waypoints[0].position;
        }
        else
        {
            Debug.LogWarning("Danh sách waypoints trống.");
        }
    }

    private void Update()
    {
        if (waypoints.Count > 0)
        {
            MoveAlongWaypoints();
        }
    }

    private void MoveAlongWaypoints()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            // Di chuyển về phía waypoint hiện tại
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Kiểm tra xem enemy đã đến gần waypoint chưa
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Enemy đã đi qua tất cả các waypoint, thực hiện hành động kết thúc nếu cần
            OnReachEnd();
        }
    }

    private void OnReachEnd()
    {
        // Thực hiện hành động khi enemy đến điểm cuối cùng
        Destroy(gameObject);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Thực hiện các hành động khi enemy bị tiêu diệt, chẳng hạn như rơi vàng
        GameManager.instance.AddGold(goldReward);
        Destroy(gameObject);
    }
}
