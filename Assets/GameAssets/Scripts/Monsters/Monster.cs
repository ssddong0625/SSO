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
using System.Diagnostics.Tracing;
using UnityEditor.Build;
using TMPro;


namespace GameAssets.Scripts.Monsters
{
    public class Monster : MonoBehaviour, IPoolable
    {
        public MonsterData data;
        [SerializeField]
       int atk;
        [SerializeField]
       float hp;
        float speed;
       int exp;
        float maxHp;
        public event Action ondie;
        public event Action bossPattern;
        public event Action<Monster>  monsterHpView;
        //public event Action<GameObject> onReturn;
        //public event Action<Spawner> onspawner;
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

       // public GameObject panel;
        public Image img;

        bool oneTime;
        public  TMP_Text text;
        [SerializeField]
        LayerMask hitLayerMask;
        //public Spawner spanwer;
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
                
                
                if (!oneTime&&hp/maxHp<=0.5f)
                {
                    oneTime= true; 
                    bossPattern?.Invoke();
                }

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
            speed = 1f;
           // panel.gameObject.SetActive(false);
          //  InitData();
            TryGetComponent(out agent);
            hits=new HashSet<IHitAble>();
            // TryGetComponent(out boxCol);
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
            if (distance <= attackRange)
            {
                StopMoving();
                if (Time.time >= nextAttack)
                {
                    nextAttack = Time.time + attackCool;
                    Attack();
                    speed = 0f;
                }
                return;
            }
            if (distance <= detectiveRange)
            {
                speed = 1f;
                ChaseTarget();
                animator.SetFloat("Walk", speed);
            }
            else
            {
                StopMoving();
                agent.SetDestination(spawnPos);
                speed = 0f;
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
            //onReturn?.Invoke(gameObject);
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
            Debug.Log("터틀이 플레이어를 맞혔습니다");
            if (((1 << other.gameObject.layer) & hitLayerMask.value) == 0) { return; }
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
            img.gameObject.SetActive(true);
        }

        public void OnDeSpawned()
        {
            hp = data.hp;
            maxHp = data.maxHp;
            exp = data.exp;
            atk = data.atk;
            //  panel.gameObject.SetActive(false);
        }
    }




}
