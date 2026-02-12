using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    public Image expImg;
    public Image runGaugeImg;
    public PlayerMove playerGauge;
    public Player player;
    public Monster monster;
    [SerializeField]
    public GameObject runGaugePanel;

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
        if (runGaugePanel != null)
        {
            runGaugePanel.SetActive(false);
        }
        GameManager.instance.onExpChanged += RefreshExpUI;
        playerGauge.onRun += CharacterRun;
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

    private void SetActive()
    {
        if (playerGauge.Gauge < playerGauge.MaxGauge)
        {
            runGaugePanel.gameObject.SetActive(true);
        }
        else
        {
            runGaugePanel.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        SetActive();
    }
    private void RefreshHp()
    {
    }

    private void CharacterRun()
    {
        runGaugeImg.fillAmount = playerGauge.Gauge / playerGauge.MaxGauge;
        playerGauge.onRun -= CharacterRun;
        playerGauge.onRun += CharacterRun;
        
    }
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
