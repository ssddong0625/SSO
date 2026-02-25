using GameAssets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatus
{
    float hp = 100f;
    float maxHp = 100f;
    float exp=0;
    int level=1;
    float levelUpExp = 100f;
    float expStep = 1.15f;
    float mp = 100f;
    float maxMp = 100f;
    



    float playerAtk = 1f;
    float playerWeaponAtk = 0f;
    public event Action onExpChanged;
    public event Action onLevelUp;
    public event Action onHpRefresh;
    public event Action onMpRefresh;
    public event Action onDie;

    public PlayerStatus()
    {
        hp = 100f;
        maxHp = hp;
        exp = 0f;
        level = 1;
        levelUpExp = 100f;
        expStep = 1.15f;
    }
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            onHpRefresh?.Invoke();
            if (hp <= 0)
            {
                onDie?.Invoke();
            }
        }
    }
    public float MaxHp => maxHp;
    public float Mp
    {
        get { return mp; }
        set { mp = value; }
    }
    public float MaxMp => maxMp;
    public float PlayerAtk
    {
        get { return playerAtk; }
        set
        {
            playerAtk = value;
        }
    }
    public float PlayerWeaponAtk
    {
        get
        {
          
            playerWeaponAtk = GameManager.instance.PlayerEquip.CurrentWeapon.atk;
            return playerAtk + playerWeaponAtk;
        }
    }
    public int Level => level;
    public float Exp => exp;
    public int NeedExp()
    {
        
        return Mathf.CeilToInt(levelUpExp * Mathf.Pow(expStep, level - 1));
    }
    public void AddExp(int amount)
    {
        exp += amount;
        
        while (exp >= NeedExp())
        {
            exp -= NeedExp();
            hp= hp + 5;
            maxHp= maxHp+5;
            hp = MaxHp;
            level++;
            onLevelUp?.Invoke();
        }

        onExpChanged?.Invoke();
    }
    
    public void Heal(float amount)
    {
        if (amount <= 0) { return; }
        Hp = Mathf.Min(Hp + amount, MaxHp);
    }

    public void Reset()
    {
        hp = 100f;
        maxHp = hp;
        mp = 100f;
        maxMp = mp;
        exp = 0f;
        level = 1;
        levelUpExp = 100f;
        expStep = 1.15f;
        onExpChanged?.Invoke();
    }
}
