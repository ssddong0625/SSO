using GameAssets.Scripts.Manager;
using UnityEngine;

public class PlayerPotionReceiver : MonoBehaviour, IPotionReceiver
{
    public void ReceivePotion(ItemData item)
    {
        if (item == null) return;
        if (item.type != ItemType.Potion) return;

        // ItemData에 healAmount 같은 값이 있어야 함(없으면 추가)
        float heal = item.healAmount;

        GameManager.instance.PlayerStauts.Heal(heal);
    }
}