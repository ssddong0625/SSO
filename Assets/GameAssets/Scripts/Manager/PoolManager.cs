using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Players;
using GameAssets.Scripts.Data;
using Unity.VisualScripting;

namespace GameAssets.Scripts.Manager
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance = null;
        Queue<GameObject> pool;
        GameObject targetObj;
        private int poolCount;
        private int spawnIndex;
        Vector3[] monsterSpawn;
        Monster monster;
        [SerializeField]
        Transform player;
        int monsterSpawnTime;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            pool = new Queue<GameObject>();
        }
        public void InitData(MonsterSpawnData data)
        {
            poolCount = data.poolCount;
            monsterSpawn = data.monsterSpawn;
            monster = data.monster;
            targetObj = data.targetObj;
            monsterSpawnTime= data.monsterSpawnTime;
        }

        // Start is called before the first frame update
        void Start()
        {
            BuildPool();
            StartCoroutine(MonsterPoolingCO());
        }

        public void BuildPool()
        {
            for(int i=0;i< poolCount; i++)
            {
                GameObject newObj=Instantiate(targetObj);
                newObj.SetActive(false);
                Monster monster = newObj.GetComponent<Monster>();
                Vector3 spawnPos = monsterSpawn[i % monsterSpawn.Length];
                monster.SpawnPos(spawnPos);
                pool.Enqueue(newObj);
            }
        }
        public void UsePool()
        {
          
            for(int s = 0; s < poolCount; s++)
            {
                if (pool == null || pool.Count == 0)
                {
                    Debug.Log("인덱스 초과");
                    return;
                }
                GameObject useObj = pool.Dequeue();
                Monster monster = useObj.GetComponent<Monster>();
                monster.Spawn();
                monster.SetTarget(player);
               
            }
           
        }
        IEnumerator MonsterPoolingCO()
        {
            while(pool!=null && pool.Count>0)
            {
                yield return new WaitForSeconds(monsterSpawnTime);
                UsePool();
            }
            Debug.Log("몬스터 풀 소환");
        }
        public void ReturnPool(GameObject returnObj)
        {
            returnObj.SetActive(false);
            pool.Enqueue(returnObj);
            if(pool != null && pool.Count > 0)
            {
                StartCoroutine(MonsterPoolingCO());
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UsePool();
            }
        }
    }
}

