using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Players;
using System;

namespace GameAssets.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public WeaponData data;
        int atk;
        //float atkSpeed;
      
        [SerializeField]
        protected LayerMask hitLayermask;
        public float attackSpeed;
        public BoxCollider hitCollider;
        private HashSet<IHitAble> hits;
        [SerializeField]
        Transform gripPoint;
        protected float nextAttack = -999f;
        protected float attackCoolDown;
       

        public Transform GripPoint => gripPoint;
        

        private void Awake()
        {
            InitData();
            
            hitCollider=GetComponent<BoxCollider>();
            hits = new HashSet<IHitAble>();
        }
        public void Start()
        {
            if (hitCollider != null)
            {
              hitCollider.isTrigger = false;
            }
            
        }
        public void InitData()
        {
            atk = data.atk;
            attackSpeed=data.weaponSpeed;
            attackCoolDown = data.attackCoolDown;
        }
        public int Atk
        {
            get { return atk; }
            set
            {
                atk = value;
            }
        }

        public void HateAttack()
        {
            /*
            if (hits == null)
            {
                Debug.Log("HasSet 널");
            }
            if (hits != null)
            {
                Debug.Log($"{hits}채워짐");
            }
            */
            hits.Clear();
        }
        
        public virtual void Attack()
        {
            
        }
        protected IEnumerator TriggerCo()
        {
            yield return new WaitForSeconds(0.2f);
            hitCollider.isTrigger = false;
        }

        public void OnTriggerEnter(Collider other)  
        {
            if (((1 << other.gameObject.layer) & hitLayermask.value) == 0) { return; }
                IHitAble hit = other.GetComponent<IHitAble>();
               if (!hits.Add(hit)) { Debug.Log("플레이어 리턴 합니다 중복체크 하세요"); return; }
                hit?.Hit(atk);
        }
    }

}

