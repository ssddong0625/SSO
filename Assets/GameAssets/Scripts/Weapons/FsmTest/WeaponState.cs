using GameAssets.Scripts.Data;
using GameAssets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponState
{
    protected WeaponStateMachine fsm;
    protected PlayerEquip playerEquip;
    protected Weapon weapon;
    protected float nextAttack;
    protected WeaponState(WeaponStateMachine fsm, PlayerEquip playerEquip)
    {
        this.fsm = fsm;
        this.playerEquip = playerEquip;
    }

  //  public abstract bool AttackCool();
   
    public abstract void Enter(WeaponData data);
    public abstract void Exit();

    public abstract void UpdateState();
    

}
