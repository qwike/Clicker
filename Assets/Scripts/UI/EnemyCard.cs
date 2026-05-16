using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EnemyCard : MonoBehaviour
{
    public Image enemyImage;
    public Image enemyBg;

    public GameObject maxHpContainer;
    public TextMeshProUGUI maxHpText;
    public GameObject damageContainer;
    public TextMeshProUGUI damageText;
    public GameObject fireDamageContainer;
    public TextMeshProUGUI fireDamageText;
    public GameObject strongDamage;
    public TextMeshProUGUI strongDamageText;

    public Canvas enemyImageBlock;

    public float duration = 1f;
    public float strength = 20f;
    Vector3 originalPos;

    public bool isBuffed = false;

    Coroutine shakeCoroutine;
    Coroutine attackCoroutine;

    void Awake()
    {
        originalPos = enemyImageBlock.transform.localPosition;
    }

    void Update()
    {
        if (isBuffed) Pulse();
    }

    public void Setup(EnemyData data)
    {
        enemyImage.sprite = data.sprite;
        enemyBg.sprite = data.background;

        maxHpText.text = data.maxHP.ToString();
        maxHpContainer.SetActive(true);
        damageText.text = data.damage.ToString();
        damageContainer.SetActive(true);

        if (data is EnemyDataF fireData)
        {
            fireDamageText.text = fireData.fireDamage.ToString();
            fireDamageContainer.SetActive(true);
        }
        if (data is BossData bossData)
        {
            strongDamageText.text = bossData.strongDamage.ToString();
            strongDamage.SetActive(true);
        }
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float time = 0f;
        

        while (time < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            enemyImageBlock.transform.localPosition = originalPos + new Vector3(x, y, 0);
            enemyImage.color = new Color(Mathf.Abs(x), 0f, 0f, 1);

            time += Time.deltaTime;
            yield return null;
        }

        enemyImageBlock.transform.localPosition = originalPos;
        enemyImage.color = Color.white;
    }

    public void Attack()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(AttackAnimation());
    }

    IEnumerator AttackAnimation()
    {
        RectTransform rect = enemyImage.rectTransform;

        Vector3 originalPos = rect.anchoredPosition;
        Vector3 attackPos = originalPos + new Vector3(0, -50, 0);

        Vector3 originalScale = rect.localScale;
        Vector3 attackScale = originalScale * 1.2f;

        float duration = 0.1f;
        float time = 0;

        while (time < duration)
        {
            float t = time / duration;

            rect.anchoredPosition = Vector3.Lerp(originalPos, attackPos, t);
            rect.localScale = Vector3.Lerp(originalScale, attackScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        while (time < duration)
        {
            float t = time / duration;

            rect.anchoredPosition = Vector3.Lerp(attackPos, originalPos, t);
            rect.localScale = Vector3.Lerp(attackScale, originalScale, t);

            time += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
        rect.localScale = originalScale;
    }

    public void Pulse()
    {
        float t = (Mathf.Sin(Time.time * 2f) + 1f) / 2f;

        Color targetColor = new Color(0.7f, 0.3f, 1f);

        enemyImage.color = Color.Lerp(Color.white, targetColor, t);
    }
}