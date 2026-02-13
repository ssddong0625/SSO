using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    public Image expImg;
    public Image runGaugeImg;
    public PlayerController playerGauge;
    public Player player;
    [SerializeField]
    public TMP_Text levelText;
    [SerializeField]
    public TMP_Text HpText;
    public Monster monster;
    [SerializeField]
    public GameObject runGaugePanel;

    [SerializeField]
    TMP_Text monsterHpTextView;
    [SerializeField]
    Image monsterHpImgView;
    //public event Action<Monster> monsterHpView;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }   
        monsterHpImgView.gameObject.SetActive(false);
    }
    public void MonsterHpView(Monster monster)
    {
        monsterHpImgView.gameObject.SetActive(true);
        monsterHpTextView.text = $"{monster.Hp}/{monster.MaxHp}";
        if (monster.Hp <= 0)
        {
            monsterHpImgView.gameObject.SetActive(false);
            
            
        }
       // monster.monsterHpView -= MonsterHpView;
        //monster.monsterHpView += MonsterHpView;
        

    }

    

    public void Start()
    {
        if (runGaugePanel != null)
        {
            runGaugePanel.SetActive(false);
        }
        GameManager.instance.onExpChanged += RefreshExpUI;
        playerGauge.onRun += CharacterRun;
        StartCoroutine(UiSettingCo());
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
    IEnumerator UiSettingCo()
    {
        yield return null;
        UpdatePlayerHpUi();
        UpdateLevel();
    }
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
    public void UpdatePlayerHpUi()
    {
        HpText.text =$"{player.HP}/{player.MaxHp}";
    }
    public void UpdateLevel()
    {
        levelText.text = "Lv"+GameManager.instance.Level;
    }
    public void Update()
    {
        SetActive();
    }
    private void RefreshHp()
    {
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
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
