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
    public event Action OnChanged;

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
                    OnChanged?.Invoke();
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
                OnChanged?.Invoke();
                return true;
            }
        }

        // 공간 부족
        OnChanged?.Invoke();
        return false;
    }

    public void ClearSlot(int index)
    {
        if (!IsValid(index)) return;

        slots[index].item = null;
        slots[index].amount = 0;

        OnChanged?.Invoke();
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

        OnChanged?.Invoke();
    }

    public void Swap(int from, int to)
    {
        if (!IsValid(from) || !IsValid(to)) return;
        if (from == to) return;

        Slot a = slots[from];
        Slot b = slots[to];

        slots[to] = a;
        slots[from] = b;

        OnChanged?.Invoke();
    }

    private bool IsValid(int index)
    {
        return slots != null && index >= 0 && index < slots.Length;
    }
}

