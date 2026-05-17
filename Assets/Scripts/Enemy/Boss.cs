using UnityEngine;

public class Boss : Enemy
{
    public BossData bossData => (BossData)data;

    public override void Attack()
    {
        // Генерация случайного числа в диапазоне от 0.0 до 100.0
        float attackType = Random.Range(0f, 100f);

        if (attackType <= 60f)
        {
            BaseAttack(); // Обычный удар (60% вероятность)
        }
        else if (attackType <= 80f)
        {
            StrongAttack(); // Сильный критический удар (20% вероятность)
        }
        else
        {
            // Распределенная во времени огненная атака (20% вероятность)
            FireAttack(bossData.fireAttacks, bossData.fireDamage);
        }
    }

    void BaseAttack()
    {
        card.Attack();
        Player.Instance.TakeDamage(bossData.damage);
    }

    void StrongAttack()
    {
        card.Attack();
        Player.Instance.TakeDamage(bossData.strongDamage);
    }

    public override void Die()
    {
        base.Die();

        UIManager.Instance.ShowVictory();
        PlayerPrefs.SetInt("isInfinityMode", 1);
        AchievementsManager.Instance.AddProgress(AchievementData.AchievementType.bossKills, 1);
    }
}