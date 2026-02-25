using GameAssets.Scripts.Monsters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BossMonster : Monster
{
    [SerializeField] private bool enableHalfPattern = true;
    [SerializeField] private float halfThreshold = 0.50f;
    [SerializeField] private UnityEvent onHalfHpPattern;
    private bool halfTriggered;



    [SerializeField] private float threePercent = 0.3f;
    [SerializeField] private int bossAtk = 2;
    [SerializeField] private float bossSpeed = 1.3f;
    [SerializeField] private float bossCool = 0.7f;
    [SerializeField] private bool enraged = false;


    public event Action bossPattern;



    public override void TakeDamage(int atk)
    {
        Debug.Log("BossMonster.TakeDamage 들어옴");
        base.TakeDamage(atk);

        OnHpChanged();
    }

    public void OnHpChanged()
    {
        if (MaxHp <= 0) { return; }
        float ratio = hp / maxHp;
        Debug.Log(ratio);

        if (enableHalfPattern && !halfTriggered && ratio <= halfThreshold)
        {
            halfTriggered = true;
            bossPattern?.Invoke();
            Debug.Log("50프로 진입");
        } 

        if (!enraged&&ratio <= threePercent)
        {
            WideWidth();
            Debug.Log("30프로 진입");
           
        }

    }
    public void WideWidth()
    {
        enraged = true;
        atk = atk* bossAtk;
        speed = speed * bossSpeed;
        attackCool= attackCool * bossCool;
        Debug.Log("30프로 진입");
    }


}
