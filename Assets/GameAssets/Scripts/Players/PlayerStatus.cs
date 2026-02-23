using GameAssets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatus
{
    [SerializeField]
    float hp = 100f;
    [SerializeField]
    float maxHp = 100f;
    [SerializeField]
    float exp=0;
    [SerializeField]
    int level=1;
    [SerializeField]
    float levelUpExp = 100f;
    [SerializeField]
    float expStep = 1.15f;

    public event Action onExpChanged;
    public event Action onLevelUp;
    public event Action onHpRefresh;
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
            maxHp=hp;
            level++;
            onLevelUp?.Invoke();
        }

        onExpChanged?.Invoke();
    }
    

    public void Reset()
    {
        hp = 100f;
        maxHp = hp;
        exp = 0f;
        level = 1;
        levelUpExp = 100f;
        expStep = 1.15f;
        onExpChanged?.Invoke();
    }
}
