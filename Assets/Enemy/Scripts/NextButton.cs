using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Button nextButton;

    private void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
    }

    private void OnNextButtonClick()
    {
        enemySpawner.NextRound();
        enemySpawner.StartRound(); // Bắt đầu vòng đấu và sinh ra enemy
    }
}
