using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private Image icon;

    private ItemData equippedItem;
    [SerializeField] private InventorySystem inventory;
    public ItemData EquippedItem => equippedItem;

    public void SetItem(ItemData item)
    {
        equippedItem = item;

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
        ItemData temp = equippedItem;
        SetItem(null);
        return temp;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) { return; }
        if(inventory != null)
        {
            inventory.UnequipWeapon();
        }
    }
}
