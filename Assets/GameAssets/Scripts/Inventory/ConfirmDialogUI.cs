using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmDialogUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action onYes;
    private Action onNo;

    private void Awake()
    {
        if (root != null) root.SetActive(false);

        yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            Close();
        });

        noButton.onClick.AddListener(() =>
        {
            onNo?.Invoke();
            Close();
        });
    }

    public void Show(string message, Action yes, Action no)
    {
        if (messageText != null) messageText.text = message;

        onYes = yes;
        onNo = no;

        if (root != null) root.SetActive(true);
    }

    private void Close()
    {
        onYes = null;
        onNo = null;

        if (root != null) root.SetActive(false);
    }
}
