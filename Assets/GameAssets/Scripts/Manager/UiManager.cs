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
   // public static UiManager instance = null;
    public Image expImg;
    public Image runGaugeImg;
    public PlayerController playerGauge;
    public Player player;
    public Monster monster;

    [Header("플레이어")]
    [SerializeField]
    Image playerHpImg;
    [SerializeField]
    public TMP_Text levelText;
    [SerializeField]
    public TMP_Text HpText;
    [SerializeField]
    public GameObject runGaugePanel;
    [SerializeField]
    Image playerSkillGauge;
    [SerializeField]
    Image skillCool;
    [SerializeField]
    TMP_Text skillCoolText;

    [Header("테스트용")]
    public Image crossHead;
    [SerializeField]
    TMP_Text monsterHpTextView;
    [SerializeField]
    Image monsterHpImgView;
    private void Awake()
    {
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
    }

    public void Start()
    {
        if (runGaugePanel != null)
        {
            runGaugePanel.SetActive(false);
        }
        GameManager.instance.PlayerStauts.onExpChanged += RefreshExpUI;
        playerGauge.onRun += CharacterRun;
        StartCoroutine(UiSettingCo());
    }
   
    IEnumerator UiSettingCo()
    {
        yield return null;
        PlayerRefreshHpUiText();
        PlayerRefreshHpUiImg();
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
    public void PlayerRefreshHpUiText()
    {
        HpText.text =$"{GameManager.instance.PlayerStauts.Hp}/{GameManager.instance.PlayerStauts.MaxHp}";
    }
    public void UpdateLevel()
    {
        levelText.text = "Lv"+GameManager.instance.PlayerStauts.Level;
    }
    public void Update()
    {
        SetActive();
        SkillCool();
    }
    private void RefreshHp()
    {
    }

    private void OnEnable()
    {
        playerGauge.onRun += CharacterRun;
        GameManager.instance.PlayerStauts.onExpChanged += RefreshExpUI;
        GameManager.instance.PlayerStauts.onHpRefresh += PlayerRefreshHpUiImg;
        GameManager.instance.PlayerStauts.onHpRefresh += PlayerRefreshHpUiText;
        GameManager.instance.Skill.onSkill+= PlayerSkillUiImg;
        GameManager.instance.Skill.useSkill += SkillCool;
    }
    private void OnDisable()
    {
        
        playerGauge.onRun -= CharacterRun;
        GameManager.instance.PlayerStauts.onExpChanged -= RefreshExpUI;
        GameManager.instance.PlayerStauts.onHpRefresh -= PlayerRefreshHpUiImg;
        GameManager.instance.PlayerStauts.onHpRefresh -= PlayerRefreshHpUiText;
        GameManager.instance.Skill.onSkill -= PlayerSkillUiImg;
        GameManager.instance.Skill.useSkill -= SkillCool;
    }
    private void CharacterRun()
    {
        runGaugeImg.fillAmount = playerGauge.Gauge / playerGauge.MaxGauge;
        
    }
    public void PlayerRefreshHpUiImg()
    {
       playerHpImg.fillAmount= GameManager.instance.PlayerStauts.Hp / GameManager.instance.PlayerStauts.MaxHp;
    }
    private void RefreshExpUI()
    {
        if (expImg == null) return;
        if (GameManager.instance == null) return;

        int need = GameManager.instance.PlayerStauts.NeedExp();
        if (need <= 0) need = 1;
        
        expImg.fillAmount = GameManager.instance.PlayerStauts.Exp / (float)need;
    }
    private void PlayerSkillUiImg()
    {
        playerSkillGauge.fillAmount = GameManager.instance.Skill.SkillGague / GameManager.instance.Skill.SkillMaxGague;
    }
    private void SkillCool()
    {
        float remain = GameManager.instance.Skill.GetRemainTIme();
        if (remain > 0f)
        {
            float ratio = remain / GameManager.instance.Skill.SkillCoolTime;
            skillCool.fillAmount = ratio;
            skillCoolText.text=Mathf.CeilToInt(remain).ToString();
            skillCoolText.gameObject.SetActive(true);
        }
        else
        {
            skillCool.fillAmount = 0f;
            skillCoolText.gameObject.SetActive(false);
        }
    }

}
