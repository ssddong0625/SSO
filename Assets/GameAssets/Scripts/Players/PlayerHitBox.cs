using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox:MonoBehaviour, IHitAble
{
    [SerializeField]
    Player player;
    

    public void Hit(int atk)
    {
        GameManager.instance.PlayerStauts.Hp -= atk;
        if (GameManager.instance.PlayerStauts.Hp <= 0)
        {
            Destroy(player.gameObject);
        }
    }
    
}
    