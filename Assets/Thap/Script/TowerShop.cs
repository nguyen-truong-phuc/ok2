using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TowerShop : MonoBehaviour
{
    public GameObject staticTowerPrefab; // Prefab của tháp đứng yên
    public GameObject rotatingTowerPrefab; // Prefab của tháp xoay
    public GameObject thirdTowerPrefab; // Prefab của tháp thứ ba
    public TextMeshProUGUI goldText; // Hiển thị số vàng
    public TextMeshProUGUI scoreText; // Hiển thị điểm
    public TextMeshProUGUI infoText; // Hiển thị thông tin tháp
    public Button buyStaticTowerButton; // Nút mua tháp đứng yên
    public Button buyRotatingTowerButton; // Nút mua tháp xoay
    public Button buyThirdTowerButton; // Nút mua tháp thứ ba
    public Tilemap towerTilemap; // Tilemap để đặt tháp
    public Button upgradeButton; // Nút nâng cấp
    public Button sellButton; // Nút bán tháp

    private GameObject towerToPlace; // Tháp sẽ được đặt
    private TowerController selectedTower; // Tháp đang được chọn để nâng cấp hoặc bán

    private void Start()
    {
        GameManager.instance.OnGoldChanged.AddListener(UpdateGoldText); // Đăng ký lắng nghe sự kiện thay đổi vàng

        buyStaticTowerButton.onClick.AddListener(() => PrepareToPlaceTower(staticTowerPrefab, 100)); // Giá 100 vàng
        buyRotatingTowerButton.onClick.AddListener(() => PrepareToPlaceTower(rotatingTowerPrefab, 150)); // Giá 150 vàng
        buyThirdTowerButton.onClick.AddListener(() => PrepareToPlaceTower(thirdTowerPrefab, 250)); // Giá 250 vàng
        upgradeButton.onClick.AddListener(UpgradeTower);
        sellButton.onClick.AddListener(SellTower);

        upgradeButton.gameObject.SetActive(false); // Ẩn nút nâng cấp
        sellButton.gameObject.SetActive(false); // Ẩn nút bán
        UpdateGoldText(GameManager.instance.GetGold());
        UpdateScoreText(GameManager.instance.GetScore());
    }

    private void PrepareToPlaceTower(GameObject towerPrefab, int cost)
    {
        if (GameManager.instance.GetGold() >= cost)
        {
            towerToPlace = towerPrefab;
            GameManager.instance.AddGold(-cost); // Trừ vàng khi chuẩn bị đặt tháp
        }
    }

    private void Update()
    {
        if (towerToPlace != null && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = towerTilemap.WorldToCell(mousePosition);

            if (towerTilemap.HasTile(cellPosition))
            {
                PlaceTower(cellPosition);
            }
        }

        if (Input.GetMouseButtonDown(1)) // Nhấn chuột phải để chọn tháp
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null && hitCollider.CompareTag("Tower"))
            {
                selectedTower = hitCollider.GetComponent<TowerController>();
                if (selectedTower != null)
                {
                    ShowTowerInfo(selectedTower);
                }
                else
                {
                    Debug.LogWarning("Đối tượng đã chọn không có component TowerController.");
                }
            }
            else
            {
                // Nếu không chọn được tháp thì ẩn bảng thông tin
                HideTowerInfo();
            }
        }
    }

    private void PlaceTower(Vector3Int cellPosition)
    {
        Vector3 cellCenter = towerTilemap.GetCellCenterWorld(cellPosition);
        GameObject placedTower = Instantiate(towerToPlace, cellCenter, Quaternion.identity);
        placedTower.GetComponent<Collider2D>().isTrigger = true; // Để tháp có thể được chọn
        towerToPlace = null;
    }

    private void ShowTowerInfo(TowerController tower)
    {
        selectedTower = tower;
        infoText.text = $"Cấp độ: {tower.Level}\nSát thương: {tower.Damage}\nChi phí nâng cấp: {GetUpgradeCost(tower.Level)}\n";
        upgradeButton.gameObject.SetActive(true); // Hiển thị nút nâng cấp
        sellButton.gameObject.SetActive(true); // Hiển thị nút bán
    }

    private void HideTowerInfo()
    {
        infoText.text = ""; // Xóa thông tin tháp
        upgradeButton.gameObject.SetActive(false); // Ẩn nút nâng cấp
        sellButton.gameObject.SetActive(false); // Ẩn nút bán
        selectedTower = null; // Xóa chọn tháp
    }

    private void UpgradeTower()
    {
        if (selectedTower != null)
        {
            int upgradeCost = GetUpgradeCost(selectedTower.Level);
            if (GameManager.instance.GetGold() >= upgradeCost)
            {
                GameManager.instance.AddGold(-upgradeCost); // Trừ vàng khi nâng cấp tháp
                selectedTower.Upgrade();
                ShowTowerInfo(selectedTower); // Cập nhật thông tin tháp sau khi nâng cấp
            }
        }
    }

    private void SellTower()
    {
        if (selectedTower != null)
        {
            int sellAmount = Mathf.RoundToInt(selectedTower.OriginalCost * 0.7f);
            GameManager.instance.AddGold(sellAmount); // Cộng vàng khi bán tháp
            Destroy(selectedTower.gameObject);
            HideTowerInfo(); // Ẩn bảng thông tin và nút
        }
    }

    private int GetUpgradeCost(int level)
    {
        switch (level)
        {
            case 1: return 100;
            case 2: return 150;
            default: return 0;
        }
    }

    private void UpdateGoldText(int newGoldAmount)
    {
        goldText.text = "Vàng: " + newGoldAmount;
    }

    private void UpdateScoreText(int newScoreAmount)
    {
        scoreText.text = "Điểm: " + newScoreAmount;
    }
}
