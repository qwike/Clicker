using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance;
    public AchievementData[] allAchievements;
    public GameObject achievementPrefab;
    public Transform content;

    void Awake() => Instance = this;

    public void RefreshList()
    {
        foreach (Transform child in content) Destroy(child.gameObject);

        foreach (var ach in allAchievements)
        {
            GameObject obj = Instantiate(achievementPrefab, content);
            obj.GetComponent<AchievementUI>().Setup(ach);
        }
    }

    public void AddProgress(AchievementData.AchievementType type, int amount)
    {
        int current = PlayerPrefs.GetInt(type.ToString(), 0);
        PlayerPrefs.SetInt(type.ToString(), current + amount);
        PlayerPrefs.Save();
    }
}