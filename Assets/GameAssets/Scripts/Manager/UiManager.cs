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
    Image playerMpImg;
    [SerializeField]
    public TMP_Text levelText;
    [SerializeField]
    public TMP_Text hpText;
    [SerializeField]
    public TMP_Text mpText;
    [SerializeField]
    public GameObject runGaugePanel;
    [SerializeField]
    GameObject skillGaugePanel;
    [SerializeField]
    Image playerSkillGauge;
    
    [SerializeField]
    Image[] skillCool;
    [SerializeField]
    TMP_Text[] skillCoolText;

    [Header("테스트용")]
    public Image crossHead;
    [SerializeField]
    TMP_Text monsterHpTextView;
    [SerializeField] TMP_Text playerHp;
    [SerializeField]
    Image monsterHpImgView;

    [Header("활 전용 스킬")]
    [SerializeField]
    GameObject skillPanel;
    [SerializeField]
    SkillSlotOpen skillSlotOpen;


    public GameObject SkillGaguePanel => skillGaugePanel;
    public GameObject SkillPanel => skillPanel;
    public SkillSlotOpen SkillSlotOpen => skillSlotOpen;
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
        PlayerRefreshMpUiImg();
        PlayerRefreshMpUiText();




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
    public void UpdateLevel()
    {
        levelText.text = "Lv"+GameManager.instance.PlayerStauts.Level;
    }
    public void Update()
    {
        SetActive();
        SkillCoolFour();
      PlayerSkillUiImg();
        RefreshSkillCoolUI();
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
        GameManager.instance.PlayerStauts.onMpRefresh += PlayerRefreshMpUiImg;
        GameManager.instance.PlayerStauts.onMpRefresh += PlayerRefreshMpUiText;
        GameManager.instance.Bow.BowSkill.onSkill+= PlayerSkillUiImg;
        GameManager.instance.Bow.BowSkill.useSkill += SkillCoolFour;
    }
    private void OnDisable()
    {
        
        playerGauge.onRun -= CharacterRun;
        GameManager.instance.PlayerStauts.onExpChanged -= RefreshExpUI;
        GameManager.instance.PlayerStauts.onHpRefresh -= PlayerRefreshHpUiImg;
        GameManager.instance.PlayerStauts.onHpRefresh -= PlayerRefreshHpUiText;
        GameManager.instance.PlayerStauts.onMpRefresh -= PlayerRefreshMpUiImg;
        GameManager.instance.PlayerStauts.onMpRefresh -= PlayerRefreshMpUiText;
        GameManager.instance.Bow.BowSkill.onSkill -= PlayerSkillUiImg;
        GameManager.instance.Bow.BowSkill.useSkill -= SkillCoolFour;
    }
    private void CharacterRun()
    {
        runGaugeImg.fillAmount = playerGauge.Gauge / playerGauge.MaxGauge;
        
    }
    public void PlayerRefreshHpUiImg()
    {
       playerHpImg.fillAmount= GameManager.instance.PlayerStauts.Hp / GameManager.instance.PlayerStauts.MaxHp;
    }
    public void PlayerRefreshHpUiText()
    {
        hpText.text =$"{GameManager.instance.PlayerStauts.Hp}/{GameManager.instance.PlayerStauts.MaxHp}";
    }
    public void PlayerRefreshMpUiImg()
    {
        
        playerMpImg.fillAmount = GameManager.instance.PlayerStauts.Mp / GameManager.instance.PlayerStauts.MaxMp;
    }
    public void PlayerRefreshMpUiText()
    {
        mpText.text = $"{GameManager.instance.PlayerStauts.Mp}/{GameManager.instance.PlayerStauts.MaxMp}";
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
        Bow bow = GameManager.instance?.PlayerEquip?.CurrentWeaponObj?.GetComponent<Bow>();
        if (!bow) return;

        playerSkillGauge.fillAmount = bow.BowSkill.SkillGague / bow.BowSkill.SkillMaxGague;
    }
    

    private void SkillCoolFour()
    {
        Bow bow = GameManager.instance?.PlayerEquip?.CurrentWeaponObj?.GetComponent<Bow>();
        if (!bow) return;

        float remain = bow.BowSkill.GetRemainTIme();
        if (remain > 0f)
        {
            float ratio = remain / bow.BowSkill.SkillCoolTime;
            skillCool[3].fillAmount = ratio;
            skillCoolText[3].text=Mathf.CeilToInt(remain).ToString();
            skillCoolText[3].gameObject.SetActive(true);
        }
        else
        {
            skillCool[3].fillAmount = 0f;
            skillCoolText[3].gameObject.SetActive(false);
        }
    }

    private void RefreshSkillCoolUI()
    {
        Bow bow = GameManager.instance?.PlayerEquip?.CurrentWeaponObj?.GetComponent<Bow>();
        if (!bow) return;

        for (int i = 0; i < 3; i++)
        {
            float remain = bow.GetRemainTIme(i);
            float dur = bow.GetSkillDuration(i);

            if (remain > 0f)
            {
                skillCool[i].gameObject.SetActive(true);
                skillCool[i].fillAmount = remain / dur;

                skillCool[i].gameObject.SetActive(true);
                skillCoolText[i].text = Mathf.CeilToInt(remain).ToString();
            }
            else
            {
                skillCool[i].gameObject.SetActive(false);
                skillCool[i].gameObject.SetActive(false);
            }
        }
    }


    private void PlayerStat()
    {
        float a = GameManager.instance.PlayerStauts.Hp;
        playerHp.text = $"{a}";
    }

}
