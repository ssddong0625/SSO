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
        //float atkSpeed;
        //float nextAttack;
        
        public Animator animator;
        [SerializeField]
        LayerMask hitLayermask;
        public float attackSpeed;
        public BoxCollider hitCollider;
        private HashSet<IHitAble> hits;


        private void Awake()
        {
            InitData();
            hitCollider=GetComponent<BoxCollider>();
            hits = new HashSet<IHitAble>();
        }
        public void Start()
        {
          //  atkSpeed = 1.2f;
            attackSpeed = 2f;
            hitCollider.isTrigger = false;
            
        }
        public void InitData()
        {
            atk = data.atk;
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
            if (hits == null)
            {
                Debug.Log("HasSet 널");
            }
            hits.Clear();
        }
        public void Attack()
        {
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackSpeed", attackSpeed);

        }
      
        public void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & hitLayermask.value) == 0) { return; }
                IHitAble hit = other.GetComponent<IHitAble>();
                Debug.Log(" 되나 ?");
               if (!hits.Add(hit)) { Debug.Log("플레이어 리턴하빈다"); return; }
                hit?.Hit(atk);
            /*
            IHitAble hit = other.GetComponent<IHitAble>();
            if (!hits.Add(hit)) { Debug.Log("리턴하빈다");  return; }
            hitCollider.isTrigger = false;
            
            hit?.Hit(atk);
            */
        }
    }

}

