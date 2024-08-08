using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int Level { get; private set; }
    public int Damage { get; private set; }
    public int OriginalCost { get; private set; }

    [SerializeField] private int maxLevel = 2; // Giả sử chỉ có 2 cấp độ
    [SerializeField] private int damageIncrement = 10; // Tăng 10 damage mỗi lần nâng cấp
    [SerializeField] private int initialDamage = 10; // Damage ban đầu
    [SerializeField] private int initialCost = 100; // Giá gốc ban đầu

    private void Start()
    {
        Level = 1;
        Damage = initialDamage;
        OriginalCost = initialCost;
    }

    public void Upgrade()
    {
        if (Level < maxLevel)
        {
            Level++;
            Damage += damageIncrement; // Tăng damage mỗi lần nâng cấp
        }
    }
}
