using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public Transform spawnPoint;
    public List<Transform> waypoints; // Danh sách các waypoint

    public int currentRound = 1; // Biến currentRound để theo dõi vòng đấu hiện tại
    private int enemiesToSpawn = 5; // Số lượng enemy sẽ xuất hiện trong vòng đấu hiện tại
    private bool canSpawn = false; // Biến điều kiện để kiểm soát khi nào enemy sẽ xuất hiện

    private void Start()
    {
        // Không sinh ra enemy tự động khi bắt đầu
    }

    public void StartRound()
    {
        canSpawn = true; // Cho phép sinh ra enemy khi người chơi nhấn nút "Tiếp Theo"
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (canSpawn)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f); // Thời gian giữa các lần sinh ra enemy
            }
            canSpawn = false; // Ngừng sinh ra enemy sau khi đã hoàn tất vòng đấu
            yield return new WaitForSeconds(1f); // Thêm thời gian để người chơi chuẩn bị cho vòng tiếp theo
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab;

        if (currentRound == 1)
        {
            enemyPrefab = enemy1Prefab;
        }
        else if (currentRound == 2)
        {
            enemyPrefab = enemy2Prefab;
        }
        else if (currentRound == 3)
        {
            enemyPrefab = enemy3Prefab;
        }
        else
        {
            // Chọn enemy ngẫu nhiên từ ba loại
            int randomIndex = Random.Range(0, 3);
            switch (randomIndex)
            {
                case 0: enemyPrefab = enemy1Prefab; break;
                case 1: enemyPrefab = enemy2Prefab; break;
                case 2: enemyPrefab = enemy3Prefab; break;
                default: enemyPrefab = enemy1Prefab; break;
            }
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.waypoints = new List<Transform>(waypoints); // Cung cấp danh sách waypoints cho enemy
        enemyScript.health += (currentRound * 10); // Tăng máu theo vòng đấu
        // Không tăng tốc độ trong vòng đấu này
    }

    public void NextRound()
    {
        currentRound++;
        enemiesToSpawn = 5 + (currentRound - 1); // Tăng số lượng enemy theo vòng đấu
        // Có thể điều chỉnh các thuộc tính khác nếu cần
    }
}
