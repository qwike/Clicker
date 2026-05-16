using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    public TMP_InputField commandInput;
    public Button sendButton;

    void Start()
    {
        sendButton.onClick.AddListener(HandleCommand);
    }

    public void HandleCommand()
    {
        string input = commandInput.text;

        if (string.IsNullOrEmpty(input)) return;

        if (input.StartsWith("addmoney"))
        {
            string[] parts = input.Split(' ');
            if (parts.Length > 1 && int.TryParse(parts[1], out int amount))
            {
                GameManager.Instance.AddMoney(amount);
            }
        }

        if (input.StartsWith("addkills"))
        {
            string[] parts = input.Split(' ');
            if (parts.Length > 1 && int.TryParse(parts[1], out int amount))
            {
                AchievementsManager.Instance.AddProgress(AchievementData.AchievementType.totalKills, amount);
            }
        }

        commandInput.text = "";
    }
}