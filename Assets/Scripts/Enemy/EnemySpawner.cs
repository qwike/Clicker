using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    public Enemy enemyPrefab;
    public Boss bossPrefab;
    public EnemyData[] allEnemies;
    public BossData bossData;

    public AudioMixerGroup sfxGroup;

    Enemy currentEnemy;

    int finalTier = 5;

    void Start()
    {
        Instance = this;
        SpawnEnemy();
    }

    public int CalculateCurrentTier(int totalKills)
    {
        if (totalKills < 100) return 0;
        else if (totalKills < 300) return 1;
        else if (totalKills < 700) return 2;
        else if (totalKills < 1250) return 3;
        else if (totalKills < 2000) return 4;
        else return 5;
    }

    public void SpawnEnemy()
    {
        int totalKills = PlayerPrefs.GetInt("totalKills", 0);

        int currentTier = CalculateCurrentTier(totalKills);

        float bossChance = (totalKills - 2500) * 0.0005f;
        bossChance = Mathf.Clamp(bossChance, 0f, 0.9f);

        EnemyData selectedEnemy;
        Enemy e;
        if (Random.value < bossChance && PlayerPrefs.GetInt("isInfinityMode", 0) == 0)
        {
            selectedEnemy = bossData;
            e = Instantiate(bossPrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {
            selectedEnemy = Instantiate(GetRandomEnemyByTier(currentTier));
            e = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);

            if (currentTier == finalTier)
            {
                e.card.isBuffed = true;
                selectedEnemy.maxHP = selectedEnemy.buffedMaxHP;
                selectedEnemy.damage = selectedEnemy.buffedDamage;
                if (selectedEnemy is EnemyDataF fireData) ((EnemyDataF)selectedEnemy).fireDamage = fireData.buffedFireDamage;
                selectedEnemy.reward = 3000;
            }
        }

        e.data = selectedEnemy;
        e.spawner = this;
        currentEnemy = e;
    }

    public void Restart()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy.gameObject);
            currentEnemy = null;
        }

        SpawnEnemy();
    }

    EnemyData GetRandomEnemyByTier(int currentTier)
    {
        List<EnemyData> pool = new List<EnemyData>();
        List<float> weights = new List<float>();

        foreach (var enemy in allEnemies)
        {
            float weight = CalculateWeight(enemy, currentTier);
            if (weight > 0)
            {
                pool.Add(enemy);
                weights.Add(weight);
            }
        }

        if (PlayerPrefs.GetInt("isInfinityMode", 0) == 1)
        {
            pool.Add(bossData);
            weights.Add(0.0015f);
        }

        float totalWeight = weights.Sum();
        float randomValue = Random.Range(0, totalWeight);
        float cursor = 0;

        for (int i = 0; i < pool.Count; i++)
        {
            cursor += weights[i];
            if (cursor >= randomValue) return pool[i];
        }

        return allEnemies[0];
    }

    float CalculateWeight(EnemyData enemy, int currentTier)
    {
        if (enemy.difficultyTier > currentTier) return 0f;

        if (enemy.difficultyTier == currentTier) return 100f * enemy.spawnRarity;

        if (enemy.difficultyTier == currentTier - 1) return 20f * enemy.spawnRarity;

        if (currentTier == finalTier) return 100f * enemy.spawnRarity;

        return 5f;
    }

    public void EnemyDied()
    {
        currentEnemy = null;

        SpawnEnemy();
    }
}