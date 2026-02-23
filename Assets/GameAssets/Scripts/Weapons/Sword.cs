using GameAssets.Scripts.Players;
using GameAssets.Scripts.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : Weapon
{

    public void OnEnable()
    {
        
    }
    public void OnDisable()
    {
        
    }
    public override void Attack()
    {
        if (Time.time >= nextAttack)
        {
            nextAttack = Time.time + attackCoolDown;
            hitCollider.isTrigger = true;
            HateAttack();
            StartCoroutine(TriggerCo());
        }
        
        
    }
}
