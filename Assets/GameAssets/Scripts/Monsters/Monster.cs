using GameAssets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
using UnityEngine.AI;
using GameAssets.Scripts.Weapons;
using UnityEngine.UI;


namespace GameAssets.Scripts.Monsters
{
    public class Monster : MonoBehaviour, IPoolable
    {
        public MonsterData data;
        [SerializeField]
       int atk;
       float hp;
       int exp;
        float maxHp;
        public event Action ondie;
        public event Action<Spawner> onspawner;
        public Animator animator;
        [SerializeField]
        BoxCollider boxCol;
        NavMeshAgent agent;
        public Transform target;
       // public Vector3 returnMonster;
        public int attackRange;
        public float attackCool;
        public float detectiveRange;
        public float exitRange;
        float nextAttack;
        HashSet<IHitAble> hits;

        public Vector3 spawnPos;

        public Image img;

        public Spawner spanwer;

        public int Atk
        {
            get { return atk; }
            set
            {
                atk = value;
            }
        }
        public float Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp <= 0)
                {
                    hp = 0;
                    boxCol.enabled = false;
                    StartCoroutine(DieCo());
                }
            }
        }
        public float MaxHp
        {
            get { return maxHp; }
            set
            {
                maxHp = value;
            }
        }
        public void Awake()
        {
          //  InitData();
            TryGetComponent(out agent);
            hits=new HashSet<IHitAble>();
           // TryGetComponent(out boxCol);
        }
        public void Start()
        {
            if (agent != null)
            {
               agent.stoppingDistance = attackRange;
            }
        }
        public void Update()
        {
            UpdateCombat();
            
            img.fillAmount = hp / maxHp;
        }
        public void UpdateCombat()
        {
            if (target == null)
            {
                StopMoving();
                return;
            }
            float distance = Vector3.Distance(transform.position,target.position);
            if (distance <= attackRange)
            {
                StopMoving();
                if (Time.time >= nextAttack)
                {
                    nextAttack = Time.time + attackCool;
                    Attack();
                }
                return;
            }
            if (distance <= detectiveRange)
            {
                ChaseTarget();
            }
            else
            {
                StopMoving();
                agent.SetDestination(spawnPos);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectiveRange);
        }
        void ChaseTarget()
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        
        public void StopMoving()
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        public void GoHome(Vector3 pos)
        {
            spawnPos = pos;
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        IEnumerator DieCo()
        {
            target = null;
            animator.SetTrigger("Die");
            GameManager.instance.AddExp(exp);
            yield return new WaitForSeconds(3f);
            PoolManager.instance.ReturnPool(gameObject);
            ondie?.Invoke();
        }
     

        public void AddExp()
        {
           // GameManager.instance.Exp += exp;
        }
        /*
        public void Hit(int atk)
        {
            Hp -= atk;
            animator.SetTrigger("TakeDamage");
            Debug.Log($"맞았습니다 남은 Hp는{Hp}");
        }
        */

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("맞혔습니다");
            IHitAble hit = other.GetComponent<IHitAble>();
            if (!hits.Add(hit))
            {
                Debug.Log(" 몬스터 리턴 합니다");
                return;
            }
            hit?.Hit(atk) ;
            
        }

        public void Attack()
        {
            if(target == null)
            {
                return;
            }
            hits.Clear();
            boxCol.isTrigger = true;
            animator.SetTrigger("Hit");
            StartCoroutine(AttackTriggerCo());
        }
        IEnumerator AttackTriggerCo()
        {
            yield return new WaitForSeconds(0.2f);
            boxCol.isTrigger = false;
            
        }
        public void OnSpawned()
        {
            hp = data.hp;
            maxHp = data.maxHp;
            exp = data.exp;
            atk = data.atk;
            boxCol.enabled = true;
        }

        public void OnDeSpawned()
        {
            hp = data.hp;
            maxHp = data.maxHp;
            exp = data.exp;
            atk = data.atk;
            hp = maxHp;
        }
    }




}
