using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public EnemyCard card;

    public Text enemyName;
    public int currentHP;
    public Slider hpBar;

    public GameObject damageTextPrefab;
    public Transform damageTextSpawn;

    public EnemySpawner spawner;

    public float attackTimer = 0f;

    void Start()
    {
        card.Setup(data);

        enemyName.text = data.enemyName;
        currentHP = data.maxHP;

        UpdateHP();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= data.attackSpeed)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        ShowDamage(damage);

        if (currentHP <= 0)
            Die();

        card.Shake();
        UpdateHP();
    }

    void ShowDamage(int damage)
    {
        GameObject obj = Instantiate(damageTextPrefab, damageTextSpawn.position, Quaternion.identity, damageTextSpawn);

        obj.transform.localPosition += new Vector3(Random.Range(-200f, 200f), Random.Range(-100f, 100f), 0f);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<DamageText>().SetDamage(damage);
    }

    void UpdateHP()
    {
        hpBar.value = (float)currentHP / data.maxHP;
    }

    public virtual void Attack()
    {
        card.Attack();
        Player.Instance.TakeDamage(data.damage);

        if (data is EnemyDataF fireData) FireAttack(fireData.fireAttacks, fireData.fireDamage);
    }

    protected void FireAttack(int n, int damage)
    {
        Player.Instance.StartCoroutine(FireAttackRoutine(n, damage));
    }

    IEnumerator FireAttackRoutine(int n, int damage)
    {
        Player.Instance.fireAnim.SetActive(true);
        for (int i = 0; i < n; i++)
        {
            Player.Instance.TakeDamage(damage);

            yield return new WaitForSeconds(0.2f);
        }
        Player.Instance.fireAnim.SetActive(false);
    }

    public virtual void Die()
    {
        if (data.deathSound)
        {
            GameObject sfxObj = new GameObject("TempSfx");
            AudioSource source = sfxObj.AddComponent<AudioSource>();
            source.clip = data.deathSound;
            source.outputAudioMixerGroup = spawner.sfxGroup;
            source.Play();
            Destroy(sfxObj, data.deathSound.length);
        }

        GameManager.Instance.AddMoney(data.reward);
        AchievementsManager.Instance.AddProgress(AchievementData.AchievementType.totalKills, 1);
        spawner.EnemyDied();
        Destroy(gameObject);
    }
}