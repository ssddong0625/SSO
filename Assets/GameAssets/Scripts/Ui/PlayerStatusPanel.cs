using GameAssets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusPanel : MonoBehaviour
{

    [SerializeField] TMP_Text playerHp;
    [SerializeField] TMP_Text playerMp;
    [SerializeField] TMP_Text playerAtk;
    [SerializeField] TMP_Text playerSpeed;

    private void Update()
    {
        playerHp.text = $"{GameManager.instance.PlayerStauts.Hp}";
        playerAtk.text = $"{GameManager.instance.PlayerStauts.PlayerWeaponAtk}";
    }



}
