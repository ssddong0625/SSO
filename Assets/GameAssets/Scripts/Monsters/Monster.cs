using GameAssets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
using UnityEngine.AI;


namespace GameAssets.Scripts.Monsters
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
        int maxHp;
        public event Action<Monster> ondie;
        public Animator animator;

        NavMeshAgent agent;
        public Transform target;
       // public Vector3 returnMonster;
        public int attackRange;
        public float attackCool;
        public float detectiveRange;
        public float exitRange;
        float nextAttack;

        public Vector3 spawnPos;

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
                    hp = maxHp;
                    StartCoroutine(DieCo());
                }
            }
        }
        public void Awake()
        {
            InitData();
            TryGetComponent(out agent);
            agent.stoppingDistance = attackRange;
        }
        public void Update()
        {
            UpdateCombat();
        }
        public IEnumerator MonsterPool()
        {
            yield return new WaitForSeconds(3f);
            PoolManager.instance.UsePool();
        }
        public void UpdateCombat()
        {
            if (target == null)
            {
                StopMoving();
                agent.SetDestination(spawnPos);
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

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void InitData()
        {
            atk = data.atk;
            exp = data.exp;
            hp = data.hp;
            maxHp=data.maxHp;
         //   attackCool = data.attackCool;
           // attackRange = data.attackRange;
        }
        public void SpawnPos(Vector3 pos)
        {
            spawnPos = pos;
        }

        
        public void Spawn()
        {
            transform.position = spawnPos;
            gameObject.SetActive(true);
        }

        IEnumerator DieCo()
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(3f);
            PoolManager.instance.ReturnPool(gameObject);
        }

        public void AddExp()
        {
            GameManager.instance.Exp += exp;
        }
        public void Hit(int atk)
        {
            Hp -= atk;
            animator.SetTrigger("TakeDamage");
            Debug.Log($"맞았습니다 남은 Hp는{Hp}");
        }

        private void OnTriggerEnter(Collider other)
        {
            IHitAble hit = other.GetComponent<IHitAble>();
            hit?.Hit(atk);
        }

        public void Attack()
        {
            animator.SetTrigger("Hit");
        }
    }




}
