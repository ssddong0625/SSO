using GameAssets.Scripts.Data;
using GameAssets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordWeaponState : WeaponState
{
    public SwordWeaponState(WeaponStateMachine fsm, PlayerEquip playerEquip) : base(fsm, playerEquip) { }

    //  public override bool AttackCool()
    //  {
    //      bool next = Time.time >= nextAttack;
    //      return next;
    //  }

    public override void Enter(WeaponData data)
    {
        playerEquip.PlayerAnimator.runtimeAnimatorController = data.controller;
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + playerEquip.CurrentWeapon.attackCoolDown;
                playerEquip.PlayerAnimator.SetTrigger("Attack");
                playerEquip.PlayerAnimator.SetFloat("AttackSpeed", 1);

            }

        }
        

    }
}
