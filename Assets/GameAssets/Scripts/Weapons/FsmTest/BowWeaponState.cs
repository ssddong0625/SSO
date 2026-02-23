using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
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
    

    public override void Enter(WeaponData data) 
    {
        
        bow=playerEquip.CurrentWeaponObj.GetComponent<Bow>();
        //bow.BowChange();
        playerEquip.PlayerAnimator.runtimeAnimatorController = data.controller;
        
       // bow.BowChange();
    }
    public override void Exit()
    {
    }
    public override void UpdateState()
    {
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
        if (aiming && Input.GetMouseButtonDown(0))
        {
            if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + bow.data.attackCoolDown;
                playerEquip.PlayerAnimator.SetTrigger("Attack");
                playerEquip.PlayerAnimator.SetFloat("AttackSpeed", 1);
            }
           // playerEquip.PlayerAnimator.SetTrigger("Attack");
           // playerEquip.PlayerAnimator.SetFloat("AttackSpeed",1);
           
        }
        
      
    }

   
}
