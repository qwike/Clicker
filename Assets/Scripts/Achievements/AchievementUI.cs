using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI progressText;
    public Button claimButton;
    public TextMeshProUGUI claimButtonText;
    public TextMeshProUGUI rewardText;

    private AchievementData data;

    public Color readyToClaimColor = new Color(1f, 0.92f, 0.016f, 1f);
    public Color claimedColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    public void Setup(AchievementData newData)
    {
        data = newData;
        titleText.text = data.achievementName;
        descText.text = data.description;
        rewardText.text = "+" + data.reward;

        UpdateUI();
    }

    public void UpdateUI()
    {
        int currentProgress = PlayerPrefs.GetInt(data.type.ToString(), 0);
        bool isClaimed = PlayerPrefs.GetInt(data.GetKey(), 0) == 1;
        bool canClaim = currentProgress >= data.goalValue && !isClaimed;

        if (canClaim) {
            claimButtonText.text = "Получить";
            background.color = readyToClaimColor;
        }
        if (isClaimed)
        {
            progressText.text = "Получено";
            claimButtonText.text = "Получено";
            background.color = claimedColor;
        }
        else
        {
            claimButtonText.text = "";
            progressText.text = $"{Mathf.Min(currentProgress, data.goalValue)} / {data.goalValue}";
        }

        claimButton.interactable = canClaim;

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimReward);
    }

    void ClaimReward()
    {
        GameManager.Instance.AddMoney(data.reward);
        PlayerPrefs.SetInt(data.GetKey(), 1);
        PlayerPrefs.Save();
        UpdateUI();
    }
}