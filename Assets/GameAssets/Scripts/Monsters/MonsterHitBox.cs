using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour,IHitAble
{
    [SerializeField]
    Monster monster;
    [SerializeField]
    LayerMask hitLayerMask;
    
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayerMask.value) == 0) { return; }
        IHitAble hit = other.GetComponent<IHitAble>();

        hit?.Hit(monster.data.atk);

    }
    */
    

    // Start is called before the first frame update

    public void Hit(int atk)
    {
        // 여차하면 파트별 데미지 구현 일단 시간 남으면 Enum방식으로 부위별 처리 가능할테니 해보자!!
        monster.TakeDamage(atk);
    }
    
}
