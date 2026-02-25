using System;
using UnityEngine;

public class PotionBarSystem : MonoBehaviour
{
    [Serializable]
    public struct Slot
    {
        public ItemData item;
        public int amount;

        public bool IsEmpty() => item == null || amount <= 0;
        public void Clear() { item = null; amount = 0; }
    }

    [Header("Refs")]
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private PlayerPotionReceiver potionReceiver;

    [Header("Potion Bar")]
    [SerializeField] private int slotCount = 4;
    [SerializeField] private int maxStackInBar = 99;

    private Slot[] slots;
    private IPotionReceiver receiver;

    public event Action onChanged;

    private void Init()
    {
        if (slots == null || slots.Length != slotCount)
            slots = new Slot[slotCount];

        if (receiver == null)
            receiver = potionReceiver as IPotionReceiver;
    }

    private void Awake() => Init();
    private void OnEnable() => Init();

    public int SlotCount { get { Init(); return slots.Length; } }

    public Slot GetSlot(int index)
    {
        Init();
        if (index < 0 || index >= slots.Length) return default;
        return slots[index];
    }

    public bool MoveAllFromInventory(int invIndex, int barIndex)
    {
        Init();

        if (inventory == null) return false;
        if (barIndex < 0 || barIndex >= slots.Length) return false;

        InventorySystem.Slot invSlot = inventory.GetSlot(invIndex);
        if (invSlot.item == null || invSlot.amount <= 0) return false;
        if (invSlot.item.type != ItemType.Potion) return false;

        ItemData potion = invSlot.item;
        int moveAmount = invSlot.amount;

        if (slots[barIndex].IsEmpty())
        {
            int add = Mathf.Min(moveAmount, maxStackInBar);
            slots[barIndex].item = potion;
            slots[barIndex].amount = add;

            inventory.RemoveAmount(invIndex, add);
            onChanged?.Invoke();
            return true;
        }

        if (slots[barIndex].item == potion)
        {
            int space = maxStackInBar - slots[barIndex].amount;
            if (space <= 0) return false;

            int add = Mathf.Min(space, moveAmount);
            slots[barIndex].amount += add;

            inventory.RemoveAmount(invIndex, add);
            onChanged?.Invoke();
            return true;
        }

        ReturnAllToInventory(barIndex);
        return MoveAllFromInventory(invIndex, barIndex);
    }

    public void UseOne(int barIndex)
    {
        Init();

        if (barIndex < 0 || barIndex >= slots.Length) return;
        if (slots[barIndex].IsEmpty()) return;

        ItemData item = slots[barIndex].item;

        if (receiver != null) receiver.ReceivePotion(item);

        slots[barIndex].amount -= 1;
        if (slots[barIndex].amount <= 0)
            slots[barIndex].Clear();

        onChanged?.Invoke();
    }

    public void ReturnAllToInventory(int barIndex)
    {
        Init();

        if (inventory == null) return;
        if (barIndex < 0 || barIndex >= slots.Length) return;
        if (slots[barIndex].IsEmpty()) return;

        ItemData item = slots[barIndex].item;
        int amount = slots[barIndex].amount;

        bool ok = inventory.AddItem(item, amount);
        if (!ok) return;

        slots[barIndex].Clear();
        onChanged?.Invoke();
    }
}