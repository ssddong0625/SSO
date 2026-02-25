using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BowWeaponState : WeaponState
{
    public BowWeaponState(WeaponStateMachine fsm, PlayerEquip playerEquip) : base(fsm, playerEquip) { }
    Bow bow;
    float skillCoolTime = 5f;
    float nextSkillTime = 0f;
     
    public bool CanUseSkill()
    {
        return Time.time >= nextSkillTime;
    }
    public void StartCoolDown()
    {
        nextSkillTime = Time.time + skillCoolTime;
    }

    public void SkillTypeOne()
    {
        if (!GameManager.instance.PlayerEquip.BowAttack) return;

    }

    public override void Enter(WeaponData data) 
    {
        GameManager.instance.UiManager.SkillPanel.SetActive(true);
        bow=playerEquip.CurrentWeaponObj.GetComponent<Bow>();
        playerEquip.PlayerAnimator.runtimeAnimatorController = data.controller;
    }
    public override void Exit()
    {
        GameManager.instance.UiManager.SkillPanel.SetActive(false);
        CanDo(false);
    }
    public void CanDo(bool real)
    {
        SkillSlotOpen slotOpen = GameManager.instance.UiManager.SkillSlotOpen;
        PlayerStatus ps = GameManager.instance.PlayerStauts;
        if (ps.Level >= 5)
        {
           slotOpen.LevelFiveOpen.SetActive(real);

        }
        if (ps.Level >= 10)
        {
            slotOpen.LevelTenOpen.SetActive(real);
        }
        if (ps.Level >= 15)
        {
            slotOpen.LevelFifthOpen.SetActive(real);

        }
        if (ps.Level >= 20)
        {
            slotOpen.LevelTwentyOpen.SetActive(real);
        }
    }
    public void LevelUseSkill()
    {
        PlayerStatus ps = GameManager.instance.PlayerStauts;
        if(ps.Level <= 5) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bow.SkillTypeOne();

        }
        if (ps.Level < 10) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bow.SkillTypeTwo();
        }
        if (ps.Level < 15) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bow.SkillTypeThree();
        }
    }

    public override void UpdateState()
    {

        CanDo(true);

        bool aiming = Input.GetMouseButton(1);
        bow.Attacking();
        bow.BowSkill.Skill();

        LevelUseSkill();
        /*
        if (Input.GetMouseButton(1))
        {
            playerEquip.PlayerAnimator.SetTrigger("Ready");
            playerEquip.PlayerAnimator.ResetTrigger("GoHome");
            GameManager.instance.UiManager.crossHead.gameObject.SetActive(true);
        }
        else
        {
            playerEquip.PlayerAnimator.SetTrigger("GoHome");
            playerEquip.PlayerAnimator.ResetTrigger("Ready");
            GameManager.instance.UiManager.crossHead.gameObject.SetActive(false);
        }
       
        bool aiming = Input.GetMouseButton(1);
        if (GameManager.instance.PlayerEquip.BowAttack)
        {
           if (aiming && Input.GetMouseButtonDown(0))
           {
               if (Time.time >= nextAttack&&GameManager.instance.PlayerEquip.BowAttack)
               {
                    
                   nextAttack = Time.time + bow.data.attackCoolDown;
                   playerEquip.PlayerAnimator.SetTrigger("Attack");
                   playerEquip.PlayerAnimator.SetFloat("AttackSpeed", 1);
               }
              // playerEquip.PlayerAnimator.SetTrigger("Attack");
              // playerEquip.PlayerAnimator.SetFloat("AttackSpeed",1);
              

           }

        }
        */





    }

   
}
