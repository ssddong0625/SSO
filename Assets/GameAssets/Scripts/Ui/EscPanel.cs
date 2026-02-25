using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscPanel : MonoBehaviour
{
    [SerializeField]
    GameObject audioPanel;
    [SerializeField]
    GameObject mousePanel;
    [SerializeField]
    GameObject escPanel;

    public void OpenAudioUI()
    {
        audioPanel.SetActive(true);
        escPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
