using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpView : MonoBehaviour
{
    public Image hpView;
    [SerializeField]
    Monster monster;

    public void RefreshPlayerHpUi()
    {
       hpView.fillAmount= monster.Hp / monster.MaxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
