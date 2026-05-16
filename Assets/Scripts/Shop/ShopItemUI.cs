using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;
    public TextMeshProUGUI buttonText;

    public GameObject damageContainer;
    public TextMeshProUGUI damageText;

    public GameObject armorContainer;
    public TextMeshProUGUI armorText;

    public GameObject maxHpContainer;
    public TextMeshProUGUI maxHpText;

    public GameObject regenContainer;
    public TextMeshProUGUI regenText;

    ItemData item;

    public void Setup(ItemData newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);

        if (PlayerPrefs.GetInt(item.GetKey(), 0) == 0)
        {
            priceText.text = item.price.ToString();
        }
        else
        {
            priceText.text = "Куплено";
            bool isEquipped = false;

            if (item is WeaponData weapon)
                isEquipped = (Player.Instance.currentWeapon != null && Player.Instance.currentWeapon.itemID == weapon.itemID);
            else if (item is ArmorData armor)
                isEquipped = (Player.Instance.currentArmor != null && Player.Instance.currentArmor.itemID == armor.itemID);

            if (isEquipped)
            {
                buttonText.text = "Экипировано";
                buyButton.interactable = false;
            }
            else
            {
                buttonText.text = "Надеть";
                buyButton.interactable = true;
            }
        }

        if (item is PotionData potion)
        {
            int currentAlount = PlayerPrefs.GetInt("Item_" + potion.itemID, 0);
            priceText.text = item.price.ToString();

            regenText.text = potion.healAmount.ToString();
            regenContainer.SetActive(true);

            if (currentAlount > 0)
            {
                buttonText.text = "Купить еще (" + currentAlount + ")";
            }
        }
        else if (item is WeaponData weapon)
        {
            damageText.text = weapon.damage.ToString();
            damageContainer.SetActive(true);
        }
        else if (item is ArmorData armor)
        {
            armorText.text = armor.armor.ToString();
            armorContainer.SetActive(true);
            maxHpText.text = armor.maxHP.ToString();
            maxHpContainer.SetActive(true);
            regenText.text= armor.regen.ToString();
            regenContainer.SetActive(true);
        }
    }

    void Buy()
    {
        ShopManager.Instance.BuyItem(item);
    }
}