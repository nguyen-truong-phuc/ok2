using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int gold = 500; // Số vàng hiện tại
    private int score = 0; // Điểm hiện tại

    public UnityEvent<int> OnGoldChanged = new UnityEvent<int>(); // Sự kiện thay đổi vàng
    public UnityEvent<int> OnScoreChanged = new UnityEvent<int>(); // Sự kiện thay đổi điểm

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged.Invoke(gold); // Gọi sự kiện thay đổi vàng
    }

    public int GetGold()
    {
        return gold;
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged.Invoke(score); // Gọi sự kiện thay đổi điểm
    }


    public int GetScore()
    {
        return score;
    }
}
