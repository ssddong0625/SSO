using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject inventoryPanel; 
    [SerializeField] private GameObject equipmentPanel; 
    [SerializeField] private GameObject mapPanel;       
    [SerializeField] private GameObject escMenuPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject mousePanel;
    [SerializeField] private GameObject playerStatPanel;
    
    private readonly List<GameObject> openList = new List<GameObject>();
    private bool isUiOpen;
    public bool IsUiOpen => isUiOpen;
    void Start()
    {
        SetPanel(inventoryPanel, false);
        SetPanel(equipmentPanel, false);
        SetPanel(mapPanel, false);
        SetPanel(escMenuPanel, false);
        openList.Clear();
        RefreshState();
    }
    public void PanelKey()
    {
        
        if (Input.GetKeyDown(KeyCode.I)) Toggle(inventoryPanel);
        if (Input.GetKeyDown(KeyCode.E)) Toggle(equipmentPanel);
        if (Input.GetKeyDown(KeyCode.M)) Toggle(mapPanel);
        if (Input.GetKeyDown(KeyCode.Escape)) OnEscape();
        if (Input.GetKeyDown(KeyCode.F1)) Toggle(playerStatPanel);
        
    }
    void Update()
    {
        PanelKey();
    }

    void OnEscape()
    {
        if (mousePanel != null && mousePanel.activeSelf)
        {
            Close(mousePanel);
            RefreshState();
            return;
        }

        if (audioPanel != null && audioPanel.activeSelf)
        {
            Close(audioPanel);
            RefreshState();
            return;
        }

        if (escMenuPanel != null && escMenuPanel.activeSelf)
        {
            Close(escMenuPanel);
            RefreshState();
            return;
        }

        if (openList.Count > 0)
        {
            CloseTop();
            RefreshState();
            return;
        }

        Open(escMenuPanel);
        RefreshState();
    }
    public void ToggleAudioButton()
    {
        Toggle(audioPanel);
        Close(escMenuPanel);
        RefreshState();
    }
    public void ToggleEscButton()
    {
        Toggle(escMenuPanel);
        Close(audioPanel);
        RefreshState();
    }
    public void ToggleMouseButton()
    {
        Toggle(mousePanel);
    }

    void Toggle(GameObject panel)
    {
        if (!panel) return;

        if (panel.activeSelf) Close(panel);
        else Open(panel);

        RefreshState();
    }
    void Open(GameObject panel)
    {
        if (!panel) return;
        SetPanel(panel, true);
        openList.Remove(panel);
        openList.Add(panel);
    }

    void Close(GameObject panel)
    {
        if (!panel) return;
        SetPanel(panel, false);
        openList.Remove(panel);
    }

    void CloseTop()
    {
        int last = openList.Count - 1;
        GameObject top = openList[last];
        SetPanel(top, false);
        openList.RemoveAt(last);
    }

    void RefreshState()
    {
        for (int i = openList.Count - 1; i >= 0; i--)
        {
            if (openList[i] == null || !openList[i].activeSelf)
                openList.RemoveAt(i);
        }

        isUiOpen = openList.Count > 0;

        if (isUiOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void SetPanel(GameObject panel, bool active)
    {
        if (panel) panel.SetActive(active);
    }
}
