using GameAssets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour, IHitAble
{
    [SerializeField]
    Player player;
    public void Hit(int atk)
    {
        player.HP -= atk;
    }
    
}
