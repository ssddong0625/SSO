using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManagerTest : MonoBehaviour
{
    public static PoolManagerTest instance = null;
    public Dictionary<GameObject, Queue<GameObject>> pools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pools= new Dictionary<GameObject, Queue<GameObject>>();
    }

    public void BuildPool(MonsterSpawnData data)
    {
        if (data == null)
        {
            Debug.Log("¿¡·¯");
            return;
        }
        if(pools.ContainsKey(data.targetObj))
        {
            return;
        }
        Queue<GameObject> q=new Queue<GameObject>();
        for(int i = 0; i < data.poolCount; i++)
        {
            GameObject newObj = Instantiate(data.targetObj);
            newObj.SetActive(false);
            Monster monster = newObj.GetComponent<Monster>();
            Vector3 spawnPos = data.monsterSpawn[i%data.monsterSpawn.Length];
            monster.SpawnPos(spawnPos);
            q.Enqueue(newObj);
          

        }
        pools.Add(data.targetObj, q);
    }
    public GameObject UsePool(MonsterSpawnData data)
    {
        Queue<GameObject> q;
        if(!pools.TryGetValue(data.targetObj,out q))
        {
            BuildPool(data);
        }
        if(q.Count == 0)
        {
            return null;
        }
        return q.Dequeue();
    }
    public void ReturnPool(MonsterSpawnData data,GameObject obj)
    {
        Queue<GameObject> q;
        if (!pools.TryGetValue(data.targetObj, out q))
        {
            BuildPool(data);
        }
        obj.SetActive(false);
        q.Enqueue(obj);

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
