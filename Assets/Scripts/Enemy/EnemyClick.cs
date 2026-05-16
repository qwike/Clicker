using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyClick : MonoBehaviour, IPointerClickHandler
{
    public Enemy enemy;

    public void OnPointerClick(PointerEventData eventData)
    {
        enemy.TakeDamage(Player.Instance.GetDamage());
    }
}