using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHP = 1000000;

    public int damage;
    public float attackSpeed;

    public int reward;

    public Sprite sprite;
    public Sprite background;
    public AudioClip deathSound;

    public int difficultyTier;
    public float spawnRarity;

    public int buffedMaxHP;
    public int buffedDamage;
}