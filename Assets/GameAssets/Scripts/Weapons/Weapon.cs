using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Players;

namespace GameAssets.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public WeaponData data;
        int atk;
        float atkSpeed;
        float nextAttack;
        public Animator animator;
        [SerializeField]
        LayerMask hitLayermask;
        public float attackSpeed;
        public BoxCollider hitCollider;
        private void Awake()
        {
            InitData();
            atkSpeed = 1.2f;
            attackSpeed = 2f;
            hitCollider=GetComponent<BoxCollider>();
            hitCollider.isTrigger = false;
        }
        public void InitData()
        {
            atk = data.atk;
            //atkSpeed = data.atkSpeed;
        }

        public void Attack()
        {
            /*
            if(Time.time>=nextAttack)
            {
                nextAttack = Time.time + atkSpeed;
                animator.SetTrigger("Attack");
                
            }
            return;
            */
            attackSpeed = 2f;
            animator.SetFloat("AttackSpeed", attackSpeed);
            animator.SetTrigger("Attack");

        }
      
        public void OnTriggerEnter(Collider other)
        {
            IHitAble hit = other.GetComponent<IHitAble>();
            if (((1 << other.gameObject.layer) & hitLayermask.value) != 0)
            {
                hit?.Hit(atk);
            }
            else
            {
                return;
            }
        }
    }

}

