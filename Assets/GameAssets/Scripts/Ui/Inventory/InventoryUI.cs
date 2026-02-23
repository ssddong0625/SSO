using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private RectTransform inventoryRect;   // 인벤토리 패널(밖 드랍 판정용)
    [SerializeField] private Transform slotRoot;            // 슬롯들이 모여있는 부모
    [SerializeField] private Canvas rootCanvas;             // 드래그 아이콘 띄울 캔버스

    [Header("Optional")]
    [SerializeField] private ConfirmDialogUI confirmUI;

    [Header("Action Targets (Optional)")]
    [Tooltip("장비 장착(무기 프리팹 교체)을 담당하는 컴포넌트")]
    [SerializeField] private PlayerEquip playerEquip;

    [Tooltip("포션 사용 효과(회복 등)를 처리할 컴포넌트. IPotionReceiver를 구현해야 합니다.")]
    [SerializeField] private MonoBehaviour potionReceiver;

    private IPotionReceiver potionReceiverCached;




    private readonly List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
    private int selectedIndex = -1;

    public Canvas RootCanvas => rootCanvas;

    private void Awake()
    {
        potionReceiverCached = potionReceiver as IPotionReceiver;
        CollectSlots();
        RefreshAll(); // 에디터에서 바로 확인용
    }

    private void OnEnable()
    {
        if (inventory != null)
            inventory.onChanged += RefreshAll;

        RefreshAll();
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.onChanged -= RefreshAll;
    }

    private void CollectSlots()
    {
        slotUIs.Clear();

        if (slotRoot == null)
        {
            Debug.LogError("[InventoryUI] slotRoot가 비었습니다.");
            return;
        }

        InventorySlotUI[] found = slotRoot.GetComponentsInChildren<InventorySlotUI>(true);

        // index 기준 정렬 (중요: siblingIndex 의존 제거)
        Array.Sort(found, (a, b) => a.Index.CompareTo(b.Index));

        for (int i = 0; i < found.Length; i++)
        {
            slotUIs.Add(found[i]);
            found[i].Bind(this);
        }
    }

    public void RefreshAll()
    {
        if (inventory == null) return;

        for (int i = 0; i < slotUIs.Count; i++)
        {
            int idx = slotUIs[i].Index;
            InventorySystem.Slot slot = inventory.GetSlot(idx);

            slotUIs[i].Render(slot.item, slot.amount);
            slotUIs[i].SetSelected(idx == selectedIndex);
        }
    }

    public void Select(int index)
    {
        selectedIndex = (selectedIndex == index) ? -1 : index;
        RefreshAll();
    }

    public bool CanDrag(int index)
    {
        InventorySystem.Slot slot = inventory.GetSlot(index);
        return slot.item != null && slot.amount > 0;
    }

    public Sprite GetIcon(int index)
    {
        InventorySystem.Slot slot = inventory.GetSlot(index);
        return (slot.item != null) ? slot.item.icon : null;
    }

    public void Swap(int from, int to)
    {
        if (inventory == null) return;
        inventory.Swap(from, to);
    }
    public void TryAction(int index)
    {
        if (inventory == null) { return; }
        InventorySystem.Slot slot = inventory.GetSlot(index);
        if (slot.item == null || slot.amount <= 0) return;
        ItemData item = slot.item;
        if (item.type == ItemType.Potion)
        {
            if (confirmUI != null)
            {
                confirmUI.Show(
                    "포션을 사용하겠습니까?",
                    yes:() => inventory.UseOne(index),
                    no: () => { }
                    );
            }
            else
            {
                inventory.UseOne(index);
            }


        }
        else if (item.type == ItemType.Equipment)
        {
            if(confirmUI != null)
            {
                confirmUI.Show(
                    "Do you want to equipment?",
                    yes: () => inventory.Equip(index),                
                    no: () => { }
                    );
            }
            else
            {
                inventory.Equip(index);
            }
        }
        else
        {
            TryDrop(index);
        }


    }

    private void EquipInternal(ItemData item)
    {
        playerEquip.Equip(item.data);
    }
    public void TryDrop(int index)
    {
        if (inventory == null) return;

        InventorySystem.Slot slot = inventory.GetSlot(index);
        if (slot.item == null || slot.amount <= 0) return;

        if (confirmUI != null)
        {
            confirmUI.Show(
                "Drop this Item?",
                yes: () => inventory.ClearSlot(index),
                no: () => { }
            );
        }
        else
        {
            inventory.ClearSlot(index);
        }
    }
    
    public void TryDropIfOutside(int index, PointerEventData eventData)
    {
        if (inventoryRect == null) return;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            inventoryRect,
            eventData.position,
            eventData.pressEventCamera
        );

        if (!inside)
            TryDrop(index);
    }
}
