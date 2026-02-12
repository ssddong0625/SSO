using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Slot Index (0 ~ n-1)")]
    [SerializeField] private int index = -1;
    public int Index => index;

    [Header("UI Refs (Inspector Drag)")]
    [SerializeField] private Image icon;
    [SerializeField] private GameObject amountRoot;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject selectFrame;

    private InventoryUI ownerUI;

    // 드래그용 아이콘
    private RectTransform dragIconRT;
    private Image dragIconImage;

    public void Bind(InventoryUI ui)
    {
        ownerUI = ui;
        SetSelected(false);

        // 아이콘은 드래그/드롭 레이캐스트를 방해하면 안 되므로 꺼두는 게 안전합니다.
        if (icon != null) icon.raycastTarget = false;
    }

    public void SetSelected(bool on)
    {
        if (selectFrame != null) selectFrame.SetActive(on);
    }

    public void Render(ItemData item, int amount)
    {
        bool hasItem = (item != null && amount > 0);

        if (icon != null)
        {
            icon.enabled = hasItem;
            icon.sprite = hasItem ? item.icon : null;
        }

        bool showAmount = hasItem && item.stackable && amount > 1;

        if (amountRoot != null) amountRoot.SetActive(showAmount);
        if (amountText != null) amountText.text = showAmount ? amount.ToString() : "";
    }

    // -------------------------
    // Input
    // -------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ownerUI == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ownerUI.Select(Index);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ownerUI.TryDrop(Index);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ownerUI == null) return;
        if (!ownerUI.CanDrag(Index)) return;

        // 드래그 아이콘 생성 (Canvas 아래)
        Canvas canvas = ownerUI.RootCanvas;
        if (canvas == null) return;

        GameObject go = new GameObject("DragIcon");
        go.transform.SetParent(canvas.transform, false);

        dragIconRT = go.AddComponent<RectTransform>();
        dragIconRT.sizeDelta = new Vector2(48f, 48f);

        dragIconImage = go.AddComponent<Image>();
        dragIconImage.raycastTarget = false;
        dragIconImage.sprite = ownerUI.GetIcon(Index);
        dragIconImage.enabled = (dragIconImage.sprite != null);

        UpdateDragIconPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconRT == null) return;
        UpdateDragIconPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIconRT != null) Destroy(dragIconRT.gameObject);
        dragIconRT = null;
        dragIconImage = null;

        // 인벤토리 패널 밖에 놓으면 드랍
        ownerUI?.TryDropIfOutside(Index, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ownerUI == null) return;

        // 드래그 시작한 슬롯이 InventorySlotUI인지 확인
        GameObject dragObj = eventData.pointerDrag;
        if (dragObj == null) return;

        InventorySlotUI from = dragObj.GetComponent<InventorySlotUI>();
        if (from == null) return;
        if (from == this) return;

        ownerUI.Swap(from.Index, this.Index);
    }

    private void UpdateDragIconPosition(PointerEventData eventData)
    {
        Canvas canvas = ownerUI != null ? ownerUI.RootCanvas : null;
        if (canvas == null) return;

        RectTransform canvasRT = canvas.transform as RectTransform;
        if (canvasRT == null) return;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );

        dragIconRT.anchoredPosition = localPos;
    }
}
