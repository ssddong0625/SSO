using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    public Image expImg;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public void Start()
    {
        GameManager.instance.onExpChanged += RefreshExpUI;
    }
    /*
    private void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onExpChanged += RefreshExpUI;
        }
        RefreshExpUI();
    }

    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onExpChanged -= RefreshExpUI;
        }
    }
    */

    private void RefreshExpUI()
    {
        if (expImg == null) return;
        if (GameManager.instance == null) return;

        int need = GameManager.instance.NeedExp();
        if (need <= 0) need = 1;

        expImg.fillAmount = GameManager.instance.Exp / (float)need;
        GameManager.instance.onExpChanged -= RefreshExpUI;
        GameManager.instance.onExpChanged += RefreshExpUI;
    }

}
