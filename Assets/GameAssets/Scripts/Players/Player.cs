using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAssets.Scripts.Player
{
    public class Player : MonoBehaviour, IHitAble
    {
        public WeaponData data;
        private int hp;
        [SerializeField]
        private int atk;
        public GameObject prefab;
        public Transform weaponHand;

        
        public int HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp <= 0)
                {
                    Destroy(gameObject);
                    
                }
            }
        }
        public void InitData()
        {
            atk = data.atk;
            prefab = data.prefab;
        }
     
        public void Equip()
        {
            Instantiate(prefab, weaponHand);
            prefab.transform.SetParent(weaponHand);
           

        }

        public void Awake()
        {
            hp = 100;
            InitData();
        }

        public void Hit(int atk)
        {
            HP -= atk;
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                    Equip();

            }
        }
    }

}

