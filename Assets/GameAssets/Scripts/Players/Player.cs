using GameAssets.Scripts.Data;
using GameAssets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAssets.Scripts.Players
{
    public class Player : MonoBehaviour, IHitAble
    {
        public WeaponData data;
        [SerializeField]
        private float hp;
        [SerializeField]
        private int atk;
        float maxHP;
        public GameObject prefab;
        public Transform weaponHand;
        public Weapon weapon;
        public PoolKey key;
         
        public float HP
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
        public float MaxHp
        {
            get { return maxHP; }
            set
            {
                maxHP = value;
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
        public void Attack()
        {
            if (weapon == null)
            {
                return;
            }
            weapon.HateAttack();
            weapon.hitCollider.isTrigger = true;
            StartCoroutine(TriggerCo());
        }
        IEnumerator TriggerCo()
        {
            yield return new WaitForSeconds(0.2f);
            weapon.hitCollider.isTrigger = false;
        }
        public void Awake()
        {
           
            
        }
        public void Start()
        {
            hp = 100;
            MaxHp = hp;
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

