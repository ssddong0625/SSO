using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Players;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class BowSkillTypeFour
{
    [Header("4번째 스킬")]
    [SerializeField]
    float skillGauge = 0f;
    [SerializeField]
    float skillMaxGauge = 100f;
    bool skillModeTest = false;
    bool next = false;
    PlayerEquip player;

    float skillCoolTime = 5f;
    float nextSkillTime = 0f;


    public event Action onSkill;
    public event Action useSkill;
    public float SkillGague => skillGauge;
    public float SkillMaxGague => skillMaxGauge;
    // Start is called before the first frame update



    public float SkillCoolTime => skillCoolTime;
    public float NextSkillTime => nextSkillTime;

    public bool CanUseSkill()
    {
        return Time.time >= nextSkillTime;
    }
    public void StartCoolDown()
    {
        nextSkillTime = Time.time + skillCoolTime;
    }

    public float GetRemainTIme()
    {
        return Mathf.Max(0f, nextSkillTime - Time.time);
    }
    public void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (GameManager.instance.PlayerStauts.Level < 20) { return; }
            if (!CanUseSkill())
            {
                return;
            }
            GameManager.instance.PlayerEquip.BowAttack = false;
            GameManager.instance.UiManager.SkillGaguePanel.SetActive(true);
            skillModeTest = !next;
            next = skillModeTest;
            useSkill?.Invoke();
            Debug.Log(skillModeTest);

        }
        if (skillModeTest)
        {

            if (Input.GetMouseButton(0))
            {
                Debug.Log($"스킬 두번쨰 진입,{skillGauge}");
                skillGauge++;
                if (skillGauge >= 100f)
                {
                    skillGauge = 100f;

                }
                onSkill?.Invoke();
            }
            else
            {
                if (skillGauge == 100)
                {
                    skillGauge = 0;
                    skillModeTest = false;
                    next = skillModeTest;
                    useSkill?.Invoke();
                    StartCoolDown();
                    GameManager.instance.PlayerEquip.PlayerAnimator.SetTrigger("Skill");
                    GameManager.instance.UiManager.SkillGaguePanel.SetActive(false);
                    GameManager.instance.PlayerEquip.BowAttack = true;
                }
                Debug.Log($"스킬 세번쨰 진입,{skillGauge}");
                skillGauge--;
                if (skillGauge <= 0)
                {
                    skillGauge = 0;
                }
                onSkill?.Invoke();
            }
        }
        else
        {
            skillGauge = 0;

        }


    }
  }
