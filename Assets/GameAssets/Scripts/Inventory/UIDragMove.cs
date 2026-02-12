using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragMove : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform targetPanel; // 움직일 패널 (InventoryPanel)
    [SerializeField] private Canvas rootCanvas;         // 이 패널이 속한 Canvas

    private Vector2 pointerOffset; // 패널 기준 마우스 클릭 위치 오프셋

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetPanel == null || rootCanvas == null) return;

        // 마우스가 패널 안에서 눌린 위치(로컬좌표)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetPanel,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // OnPointerDown에서 오프셋을 이미 계산해둠
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (targetPanel == null || rootCanvas == null) return;

        RectTransform canvasRect = rootCanvas.transform as RectTransform;

        Vector2 canvasLocalPos;
        // 마우스 위치를 Canvas 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out canvasLocalPos
        );

        // 패널 pivot 기준 anchoredPosition 설정 (클릭 오프셋 보정)
        targetPanel.anchoredPosition = canvasLocalPos - pointerOffset;
    }
}

