using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private int index;
    [SerializeField] private PotionBarSystem bar;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    [Header("Hotkey (optional)")]
    [SerializeField] private KeyCode hotkey = KeyCode.None;

    private void OnEnable()
    {
        if (bar != null) bar.onChanged += Refresh;
        StartCoroutine(RefreshNextFrame());
    }

    private void OnDisable()
    {
        if (bar != null) bar.onChanged -= Refresh;
    }

    private IEnumerator RefreshNextFrame()
    {
        yield return null;
        Refresh();
    }

    private void Update()
    {
        if (bar == null) return;

        if (hotkey != KeyCode.None && Input.GetKeyDown(hotkey))
            bar.UseOne(index);
    }

    public void Refresh()
    {
        if (bar == null) return;

        var slot = bar.GetSlot(index);
        bool has = slot.item != null && slot.amount > 0;

        if (icon != null)
        {
            icon.enabled = has;
            icon.sprite = has ? slot.item.icon : null;
        }

        if (amountText != null)
            amountText.text = has ? slot.amount.ToString() : "";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (bar == null) return;

        GameObject dragObj = eventData.pointerDrag;
        if (dragObj == null) return;

        InventorySlotUI fromInv = dragObj.GetComponent<InventorySlotUI>();
        if (fromInv == null) return;

        bar.MoveAllFromInventory(fromInv.Index, index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (bar == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
            bar.UseOne(index);
        else if (eventData.button == PointerEventData.InputButton.Right)
            bar.ReturnAllToInventory(index);
    }
}