using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneWeaponState : WeaponState
{
    public NoneWeaponState(WeaponStateMachine fsm, PlayerEquip playerEquip) : base(fsm, playerEquip) { }
   

    public override void Enter(WeaponData data)
    {
        
        playerEquip.PlayerAnimator.runtimeAnimatorController = data.controller;
        Debug.Log("아직 준비안댐");
    }

    public override void Exit()
    {
        Debug.Log("아직 준비안댐22");
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          playerEquip.PlayerAnimator.SetTrigger("Attack");
          playerEquip.PlayerAnimator.SetFloat("AttackSpeed", 1);

        }
    }
}


