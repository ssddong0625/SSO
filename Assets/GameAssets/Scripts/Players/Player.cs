using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Scripts.Players
{
    public class Player : MonoBehaviour//, IHitAble
    {
      //  public WeaponData data;
        [SerializeField]
        private float hp;
        Animator animator;
        public PlayerStatus player;
        float maxHp;
        //public GameObject prefab;
        public Transform weaponHand;
        Weapon weapon;
       // public PoolKey key;
        public Image playerHp;
        public TextMeshPro text;
       // public float HP
       // {
       //     get { return hp; }
       //     set
       //     {
       //         hp = value;
       //         playerHp.fillAmount = hp / maxHp;
       //         GameManager.instance.UiManager.PlayerRefreshHpUiText();
       //         if (hp <= 0)
       //         {
       //             Destroy(gameObject);
       //         }
       //     }
       // }
        public float MaxHp
        {
            get { return maxHp; }
            set
            {
                maxHp = value;
            }
        }
        /*
        public float Exp
        {
            get { return exp; }
            set { exp = value; }
        }
        */
        //public void InitData()
        //{
        //    atk = data.atk;
        //    prefab = data.prefab;
        //}
        /* public void Equip()
         {
             Instantiate(prefab, weaponHand);
             prefab.transform.SetParent(weaponHand);
         }
        */
        public Animator Animator => animator;
      
        public void Attack()
        {
            weapon = GetComponentInChildren<Weapon>();
            if (weapon == null)
            {
                return;
            }
            weapon.HateAttack();
            weapon.hitCollider.isTrigger = true;
        //    onAttack?.Invoke();
            StartCoroutine(TriggerCo());
            
        }
        IEnumerator TriggerCo()
        {
            yield return new WaitForSeconds(0.2f);
            weapon.hitCollider.isTrigger = false;
        }
        public void Awake()
        {
            animator= GetComponent<Animator>();
         //  weapon = GetComponentInChildren<Weapon>();
        }

        public void Update()
        {
            GameManager.instance.Skill.Skill();
        }
        public void Start()
        {
          //  hp = 100;
            //MaxHp = hp;
            //InitData();
        }

        /*
        public void Hit(int atk)
        {
            player.Hp -= atk;
            if (player.Hp <= 0)
            {
                Destroy(gameObject);
            }
            
        }
        */
      
     

    }
}

