using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryUI inventoryUI;

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.I)){ return; }
        bool next = !inventoryPanel.activeSelf;
        if (next)
        {
            inventoryPanel.SetActive(next);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            inventoryPanel.SetActive(next);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (next && inventoryUI != null)
            inventoryUI.RefreshAll();
    }
}
