using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRightHand : MonoBehaviour, IHitAble
{
    [SerializeField]
    LayerMask hitLayerMask;
    [SerializeField]
    Monster monster;

    public void Hit(int atk)
    {
        monster.Hp -= atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayerMask.value) == 0) { return; }
        IHitAble hit = other.GetComponent<IHitAble>();
        
        hit?.Hit(monster.data.atk);

    }
   
}
