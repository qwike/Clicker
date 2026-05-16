using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float speed = 1.5f;
    public float lifeTime = 1f;

    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetDamage(int damage)
    {
        text.text = damage.ToString();
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            Destroy(gameObject);
    }
}