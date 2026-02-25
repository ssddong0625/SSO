using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionEquip : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;

    private ItemData potion;
    [SerializeField] private InventorySystem inventory;
    public ItemData Potion => potion;

    public void SetItem(ItemData item)
    {
        potion = item;

        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }


    public ItemData RemoveItem()
    {
        ItemData temp = potion;
        SetItem(null);
        return temp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) { return; }
        if (inventory != null)
        {
            inventory.UnequipWeapon();
        }
    }
}
