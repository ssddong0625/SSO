using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private BuildControllerSM controller;
    [SerializeField] private Text reasonText;

    private void Awake()
    {
        if (controller == null) controller = FindObjectOfType<BuildControllerSM>();
    }

    private void OnEnable()
    {
        if (controller != null)
            controller.OnReasonChanged += HandleReasonChanged;
    }

    private void OnDisable()
    {
        if (controller != null)
            controller.OnReasonChanged -= HandleReasonChanged;
    }

    private void HandleReasonChanged(string reason)
    {
        if (reasonText == null) return;

        bool show = !string.IsNullOrEmpty(reason);
        reasonText.gameObject.SetActive(show);
        reasonText.text = reason;
    }
}