using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public ItemData[] items;
    public Transform content;
    public GameObject itemPrefab;
    public enum ShopFilter { All, Weapons, Armor, Potions }
    public ShopFilter currentFilter = ShopFilter.All;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateShop(int filter)
    {
        currentFilter = (ShopFilter) filter;
        GenerateShop();
    }

    public void GenerateShop()
    {
        foreach (Transform child in content) Destroy(child.gameObject);

        foreach (var item in items)
        {
            if (currentFilter == ShopFilter.Weapons && !(item is WeaponData)) continue;
            if (currentFilter == ShopFilter.Armor && !(item is ArmorData)) continue;
            if (currentFilter == ShopFilter.Potions && !(item is PotionData)) continue;

            var obj = Instantiate(itemPrefab, content);
            obj.GetComponent<ShopItemUI>().Setup(item);
        }
    }

    public void BuyItem(ItemData item)
    {
        if (item is PotionData potion)
        {
            if (GameManager.Instance.money < potion.price) return;

            GameManager.Instance.AddMoney(-potion.price);
            string key = "Item_" + potion.itemID;
            int currentAmount = PlayerPrefs.GetInt(key, 0);
            PlayerPrefs.SetInt(key, currentAmount + 1);
            PlayerPrefs.Save();

            GenerateShop();
            UIManager.Instance.UpdateAllPotionTexts();
            return;
        }

        if (PlayerPrefs.GetInt(item.GetKey(), 0) == 0)
        {
            if (GameManager.Instance.money < item.price) return;

            AchievementsManager.Instance.AddProgress(AchievementData.AchievementType.itemsBought, 1);

            GameManager.Instance.AddMoney(-item.price);
            PlayerPrefs.SetInt("Item_" + item.itemID, 1);
            PlayerPrefs.Save();

            ApplyItem(item);
        }
        else
        {
            ApplyItem(item);
        }

        GenerateShop();
    }

    void ApplyItem(ItemData item)
    {
        if (item is WeaponData weapon)
        {
            Player.Instance.EquipWeapon(weapon);
        }
        else if (item is ArmorData armor)
        {
            Player.Instance.EquipArmor(armor);
        }
    }
}