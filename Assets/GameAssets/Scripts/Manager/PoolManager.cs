using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Players;

namespace GameAssets.Scripts.Manager
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance = null;
        Queue<GameObject> pool;
        public GameObject targetObj;
        public int poolCount;
        public Vector3 monsterSpawn;
        public Vector3 monsterSpawn2;
        [SerializeField]
        Monster monster;
        [SerializeField]
        Transform player;
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
        }

        // Start is called before the first frame update
        void Start()
        {
            pool = new Queue<GameObject>();
            poolCount = 2;
            for(int i=0;i<poolCount; i++)
            {
                GameObject newObj = Instantiate(targetObj);
                newObj.SetActive(false);
                pool.Enqueue(newObj);
                StartCoroutine(MonsterPoolingCO());
            }
         
        }
        
        public void UsePool()
        {
            GameObject useObj = pool.Dequeue();
            useObj.transform.position=monsterSpawn;
            useObj.SetActive(true);
            Monster m = useObj.GetComponent<Monster>();
            if (m != null)
            {
                m.SetTarget(player);
            }

        }

        IEnumerator MonsterPoolingCO()
        {
            yield return new WaitForSeconds(3f);
            UsePool();
            
        }
        public void ReturnPool(GameObject returnObj)
        {
            returnObj.SetActive(false);
            pool.Enqueue(returnObj);
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

