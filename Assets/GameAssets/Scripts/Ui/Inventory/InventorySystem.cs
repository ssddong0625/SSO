using System;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Serializable]
    public struct Slot
    {
        public ItemData item;
        public int amount;

        public bool IsEmpty()
        {
            return item == null || amount <= 0;
        }
    }

    [SerializeField] private int slotCount = 30;
    private Slot[] slots;

    // 실무형: 데이터 바뀌면 UI가 구독해서 전체 갱신
    public event Action onChanged;

    public event Action<ItemData> onUsePotion;
    public event Action<ItemData> onEquipItem;
    [SerializeField]
    private EquipmentSlotUI weaponSlot;
    [SerializeField] private PlayerEquip playerEquip;

    private void Awake()
    {
        slots = new Slot[slotCount];
    }

    public int SlotCount => slots != null ? slots.Length : 0;

    public Slot GetSlot(int index)
    {
        if (!IsValid(index)) return default;
        return slots[index];
    }

    public bool AddItem(ItemData data, int amount)
    {
        if (data == null) return false;
        if (amount <= 0) return false;

        // 1) 스택 가능한 경우: 기존 스택부터 채우기
        if (data.stackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != data) continue;
                if (slots[i].amount >= data.maxStack) continue;

                int space = data.maxStack - slots[i].amount;
                int add = Mathf.Min(space, amount);

                slots[i].amount += add;
                amount -= add;

                if (amount <= 0)
                {
                    onChanged?.Invoke();
                    return true;
                }
            }
        }

        
        // 2) 빈 슬롯에 넣기
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty()) continue;

            int add = data.stackable ? Mathf.Min(data.maxStack, amount) : 1;

            slots[i].item = data;
            slots[i].amount = add;

            amount -= add;

            if (amount <= 0)
            {
                onChanged?.Invoke();
                return true;
            }
        }
        

        // 공간 부족
        onChanged?.Invoke();
        return false;
    }

    public void ClearSlot(int index)
    {
        if (!IsValid(index)) return;

        slots[index].item = null;
        slots[index].amount = 0;

        onChanged?.Invoke();
    }

    public void RemoveAmount(int index, int removeAmount)
    {
        if (!IsValid(index)) return;
        if (removeAmount <= 0) return;
        if (slots[index].IsEmpty()) return;

        slots[index].amount -= removeAmount;

        if (slots[index].amount <= 0)
        {
            slots[index].item = null;
            slots[index].amount = 0;
        }

        onChanged?.Invoke();
    }
    public void UseOne(int index)
    {
        if (!IsValid(index)) return;
        Slot slot = slots[index];
        if (slot.item == null || slot.amount <= 0) return;
        if (slot.item.type != ItemType.Potion) return;
        onUsePotion?.Invoke(slot.item);
        RemoveAmount(index, 1);
    }
    /*
    public void Equip(int index)
    {
        if(!IsValid(index)) return; 
        Slot slot =slots[index];
        if (slot.item == null || slot.amount <= 0) return;
        if (slot.item.type != ItemType.Equipment) return;
        ItemData newItem = slot.item;
        if (weaponSlot.EquippedItem != null)
        {
            ItemData oldItem = weaponSlot.RemoveItem();
            AddItem(oldItem, 1);
        }
        if (playerEquip != null)
        {
            playerEquip.Equip(newItem.data);
        }
        
        weaponSlot.SetItem(newItem);
        ClearSlot(index);
        onEquipItem?.Invoke(newItem);
    }
    */
    public void Equip(int index)
    {
        if (!IsValid(index)) return;

        Slot slot = slots[index];
        if (slot.item == null || slot.amount <= 0) return;
        if (slot.item.type != ItemType.Equipment) return;

        ItemData newItem = slot.item;

        ItemData oldEquipped = weaponSlot != null ? weaponSlot.EquippedItem : null;

        ClearSlot(index);

        if (oldEquipped != null)
        {
            weaponSlot.RemoveItem();

            bool returned = AddItem(oldEquipped, 1);

            if (!returned)
            {
                weaponSlot.SetItem(oldEquipped);
                if (playerEquip != null)
                    playerEquip.Equip(oldEquipped.data);

                slots[index].item = newItem;
                slots[index].amount = 1;
                onChanged?.Invoke();
                return;
            }
        }

        if (playerEquip != null)
            playerEquip.Equip(newItem.data);

        weaponSlot.SetItem(newItem);

        onEquipItem?.Invoke(newItem);
        onChanged?.Invoke(); 
    }
    public void UnequipWeapon()
    {
        ItemData equipped = null;

        if (weaponSlot != null)
        {
            equipped = weaponSlot.EquippedItem;
        }

        if (equipped == null)
        {
            return;
        }

        bool ok = AddItem(equipped, 1);

        if (ok == false)
        {
            Debug.Log("인벤이 가득 차서 해제 불가");
            return;
        }

        if (weaponSlot != null)
        {
            weaponSlot.RemoveItem();
        }

        if (playerEquip != null)
        {
            playerEquip.Unequip();
        }

        if (onChanged != null)
        {
            onChanged.Invoke();
        }
    }


    public void Swap(int from, int to)
    {
        if (!IsValid(from) || !IsValid(to)) return;
        if (from == to) return;

        Slot a = slots[from];
        Slot b = slots[to];

        slots[to] = a;
        slots[from] = b;

        onChanged?.Invoke();
    }

    private bool IsValid(int index)
    {
        return slots != null && index >= 0 && index < slots.Length;
    }
}

