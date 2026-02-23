using UnityEngine;

public class UIToggle : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject EquipmentPanel;
    [SerializeField] private GameObject EquipmentUI;
    [SerializeField] private GameObject WorldMapPanel;

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        if(EquipmentPanel!=null)EquipmentPanel.SetActive(false);
        if (WorldMapPanel != null) WorldMapPanel.SetActive(false);
    }
    public void InventoryOC()
    {
        if (!Input.GetKeyDown(KeyCode.I)) { return; }
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
            if (next && inventoryUI != null)
                inventoryUI.RefreshAll();
        }
    }
    public void EquipmentOC()
    {
        if (!Input.GetKeyDown(KeyCode.E)) { return; }
        bool next = !EquipmentPanel.activeSelf;
        if (next)
        {
            EquipmentPanel.SetActive(next);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            EquipmentPanel.SetActive(next);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void WorldMapOC()
    {
        if (!Input.GetKeyDown(KeyCode.M)) { return; }
        bool next = !WorldMapPanel.activeSelf;
        if (next)
        {
            WorldMapPanel.SetActive(next);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            WorldMapPanel.SetActive(next);
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    private void Update()
    {
       InventoryOC();
        EquipmentOC();
        WorldMapOC();
    }
}
