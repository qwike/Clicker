using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money;
    public int totalMoney;

    public Text moneyText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        money = PlayerPrefs.GetInt("money", 0);
        if (moneyText != null)
            moneyText.text = money.ToString();
        totalMoney = PlayerPrefs.GetInt("totalMoney", 0);
    }

    public void Restart()
    {
        money = PlayerPrefs.GetInt("money", 0);
        if (moneyText != null)
            moneyText.text = money.ToString();
        totalMoney = PlayerPrefs.GetInt("totalMoney", 0);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        
        moneyText.text = money.ToString();

        PlayerPrefs.SetInt("money", money);

        if (amount > 0)
        {
            totalMoney += amount;
            PlayerPrefs.SetInt("totalMoney", totalMoney);
        }
        PlayerPrefs.Save();
    }
}