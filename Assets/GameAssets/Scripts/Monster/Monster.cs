using GameAssets.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;


namespace GameAssets.Scripts.Monster
{
    public class Monster : MonoBehaviour, IHitAble
    {
        public MonsterData data;
        [SerializeField]
        protected int atk;
        [SerializeField]
        protected int hp;
        [SerializeField]
        protected int exp;
        public event Action<Monster> ondie;
        public int Atk
        {
            get { return atk; }
            set
            {
                atk = value;

            }
        }
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp <= 0)
                {

                    AddExp();
                    Destroy(gameObject);

                }
            }
        }
        public void InitData()
        {
            atk = data.atk;
            exp = data.exp;
            hp = data.hp;
        }

        public void AddExp()
        {
            GameManager.instance.Exp += exp;
        }
        public void Hit(int atk)
        {
            Hp -= atk;
            Debug.Log($"맞았습니다 남은 Hp는{Hp}");
        }

        // Start is called before the first frame update
        void Start()
        {
            exp = 50;
            hp = 100;
        }
        
    }

}
