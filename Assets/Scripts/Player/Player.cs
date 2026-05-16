using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int baseDamage = 1;
    public int baseArmor = 0;
    public int baseRegen = 1;

    public WeaponData currentWeapon;
    public ArmorData currentArmor;

    public int maxHP = 100;
    public int currentHP;
    public Text textHP;

    public Slider hpBar;
    public float healTimer = 0.2f;

    public GameObject fireAnim;

    public Image weaponIcon;
    public Image armorIcon;
    Color showIcon = Color.white;
    Color hideIcon = new Color(1, 1, 1, 0);

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (UIManager.Instance.isArenaPanel)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= 1.2)
            {
                if (currentHP > maxHP - 1) return;
                currentHP += GetRegen();
                UpdateHP();
                healTimer = 0.2f;
            }
        }
    }

    void Start()
    {
        currentHP = PlayerPrefs.GetInt("currentHP", maxHP);
        LoadEquipment();
        UpdateHP();
        UIManager.Instance.UpdateAllPotionTexts();
    }

    public void Restart()
    {
        currentWeapon = null;
        currentArmor = null;
        weaponIcon.color = hideIcon;
        armorIcon.color = hideIcon;
        weaponIcon.sprite = null;
        armorIcon.sprite = null;
        maxHP = 100;
        currentHP = maxHP;
        textHP.text = $"{currentHP} / {maxHP}";
        UpdateHP();
        UIManager.Instance.UpdateAllPotionTexts();
    }

    public int GetDamage()
    {
        return (currentWeapon != null ? currentWeapon.damage : baseDamage);
    }

    public int GetArmor()
    {
        return (currentArmor != null? currentArmor.armor : baseArmor);
    }

    public int GetRegen()
    {
        return (currentArmor != null ? currentArmor.regen : baseRegen);
    }

    public void EquipWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;
        weaponIcon.color = showIcon;
        weaponIcon.sprite = currentWeapon.icon;
        PlayerPrefs.SetInt("equippedWeapon", weapon.itemID);
        PlayerPrefs.Save();
    }

    public void EquipArmor(ArmorData armor)
    {
        currentArmor = armor;
        armorIcon.color = showIcon;
        armorIcon.sprite = currentArmor.icon;
        maxHP = armor.maxHP;
        UpdateHP();
        PlayerPrefs.SetInt("equippedArmor", armor.itemID);
        PlayerPrefs.Save();
    }

    void LoadEquipment()
    {
        currentWeapon = null;
        currentArmor = null;
        weaponIcon.color = hideIcon;
        armorIcon.color = hideIcon;
        weaponIcon.sprite = null;
        armorIcon.sprite = null;

        int savedWeaponID = PlayerPrefs.GetInt("equippedWeapon", -1);
        int savedArmorID = PlayerPrefs.GetInt("equippedArmor", -1);

        foreach (var item in ShopManager.Instance.items)
        {
            if (item.itemID == savedWeaponID && item is WeaponData w)
                EquipWeapon((WeaponData) item);

            if (item.itemID == savedArmorID && item is ArmorData a)
                EquipArmor((ArmorData) item);
        }

        ShopManager.Instance.GenerateShop();
    }

    public void UsePotion(int potionId)
    {
        foreach (var item in ShopManager.Instance.items)
        {
            if (!(item is PotionData potion)) continue;
            if (potionId == potion.itemID)
            {
                string key = "Item_" + potionId;
                int currentAmount = PlayerPrefs.GetInt(key, 0);
                if (currentAmount < 1) return;

                PlayerPrefs.SetInt(key, currentAmount - 1);
                PlayerPrefs.Save();

                currentHP = Mathf.Min(currentHP + potion.healAmount, maxHP);
                UpdateHP();
                UIManager.Instance.UpdateAllPotionTexts();
                ShopManager.Instance.GenerateShop();
                
                return;
            }
        }
    }

    void UpdateHP()
    {
        if (currentHP > maxHP) currentHP = maxHP;
        hpBar.value = (float)currentHP / maxHP;
        textHP.text = $"{currentHP} / {maxHP}";
        PlayerPrefs.SetInt("currentHP", currentHP);
        PlayerPrefs.Save();
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - GetArmor(), 1);

        currentHP -= finalDamage;

        if (currentHP <= 0)
            Die();

        UpdateHP();
    }

    void Die()
    {
        UIManager.Instance.NewGame();
        UIManager.Instance.ShowDied();
    }
}