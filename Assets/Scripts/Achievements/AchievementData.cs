using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class AchievementData : ScriptableObject
{
    public int achievementID;
    public string achievementName;
    public string description;
    public int goalValue;
    public int reward;

    public enum AchievementType { totalKills, totalMoney, itemsBought, bossKills }
    public AchievementType type;

    public string GetKey() => "Achievement_" + achievementID;
}
