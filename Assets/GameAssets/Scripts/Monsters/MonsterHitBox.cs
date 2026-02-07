using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour,IHitAble
{
    [SerializeField]
    Monster monster;
    // Start is called before the first frame update


    public void Hit(int atk)
    {
        monster.Hp -= atk;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
