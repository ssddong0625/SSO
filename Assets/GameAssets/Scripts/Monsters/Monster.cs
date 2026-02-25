using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Players;
using GameAssets.Scripts.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace GameAssets.Scripts.Monsters
{
    public class Monster : MonoBehaviour, IPoolable
    {
        public MonsterData data;
        [SerializeField]
     protected  int atk;
        [SerializeField]
      protected float hp;
      protected  float speed;
      protected int exp;
      protected  float maxHp;
        public event Action ondie;
        public Animator animator;
        [SerializeField]
       protected BoxCollider boxCol;
       protected NavMeshAgent agent;
        public Transform target;
        public int attackRange;
        public float attackCool;
        public float detectiveRange;
        public float exitRange;
        [SerializeField]
        protected float nextAttack = -999f;
        protected HashSet<IHitAble> hits;
        public Vector3 spawnPos;

        public Transform lastAttacker;
        public Image img;

      //  bool oneTime;
        public  TMP_Text text;
        [SerializeField]
       protected LayerMask hitLayerMask;
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
                img.fillAmount = hp / maxHp;
                animator.SetTrigger("TakeDamage");
                
                
             //   if (!oneTime&&hp/maxHp<=0.5f)
             //   {
             //       oneTime= true; 
             //      // bossPattern?.Invoke();
             //   }

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
            speed = 0f;
            TryGetComponent(out agent);
            hits=new HashSet<IHitAble>();

            img.gameObject.SetActive(true);
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
            img.fillAmount = hp / maxHp;
            UpdateCombat();
        }
        public void UpdateCombat()
        {
            if (target == null)
            {
                StopMoving();
                return;
            }
            float distance = Vector3.Distance(transform.position,target.position);
          
            if (lastAttacker == null)
            {
                if (distance <= detectiveRange)
                {
                    ChaseTarget();
                    animator.SetFloat("Walk", speed);
                    if (distance <= attackRange)
                    {
                        StopMoving();
                        Attack();
                        speed = 0f;
                       

                    }


                }
                else
                {
                    StopMoving();
                    agent.SetDestination(spawnPos);
                    speed = 1f;
                }

            }
            else
            {
                ChaseTarget();
                animator.SetFloat("Walk", speed);
                if (distance <= attackRange)
                {
                    StopMoving();
                    Attack();
                    speed = 0f;
                }
            }
      
        }
        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectiveRange);
        }
        protected void ChaseTarget()
        {
            speed = 1f;
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        
        public void StopMoving()
        {
            speed = 0f;
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
        protected IEnumerator DieCo()
        {
            target = null;
            animator.SetTrigger("Die");
            GameManager.instance.PlayerStauts.AddExp(exp);
            yield return new WaitForSeconds(3f);
            PoolManager.instance.ReturnPool(gameObject);
            int randIndex = UnityEngine.Random.Range(0, data.dropItem.Length);
            Instantiate(data.dropItem[randIndex],transform.position, Quaternion.identity);
            ondie?.Invoke();
        }
     
        IEnumerator StopMovingCo()
        {
            yield return new WaitForSeconds(attackCool);
            speed = 1f;
        }

        public void HitPlayer(Collider other)
        {
            if (((1 << other.gameObject.layer) & hitLayerMask.value) == 0) { return; }
            IHitAble hit = other.GetComponent<IHitAble>();
            if (!hits.Add(hit))
            {
                return;
            }
            hit?.Hit(atk);
        }


        protected void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & hitLayerMask.value) == 0) { return; }
            IHitAble hit = other.GetComponent<IHitAble>();
            if (!hits.Add(hit))
            {
                return;
            }
            hit?.Hit(atk) ;
            
        }
        

        public void Attack()
        {
            if (target == null)
            {
                return;
            }
            if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + attackCool;
                hits.Clear();
                boxCol.isTrigger = true;
                animator.SetTrigger("Hit");
                speed = 0f;
                StartCoroutine(AttackTriggerCo());

            }
            
            
           // hits.Clear();
           // boxCol.isTrigger = true;
           // animator.SetTrigger("Hit");
           // speed = 0f;
           // StartCoroutine(AttackTriggerCo());
        }
        protected IEnumerator AttackTriggerCo()
        {
            yield return new WaitForSeconds(0.5f);
            boxCol.isTrigger = false;
            
        }
        public void OnSpawned()
        {
            hp = data.hp;
            maxHp = data.maxHp;
            exp = data.exp;
            atk = data.atk;
            attackCool= data.attackCool;
            boxCol.enabled = true;
            img.gameObject.SetActive(true);
        }

        public void OnDeSpawned()
        {
            //hp = data.hp;
            //maxHp = data.maxHp;
            //exp = data.exp;
            //atk = data.atk;
            //attackCool = data.attackCool;
            lastAttacker = null;
          
        }
        public virtual void TakeDamage(int atk)
        {
            Hp -= atk;
            GameManager.instance.UiManager.MonsterHpView(this);
            
            
        }

        public void SetAttacker(Transform attacker)
        {
            lastAttacker= attacker;
        }

    }




}
