using GameAssets.Scripts.Data;
using GameAssets.Scripts.Monsters;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    PoolKey key;
    [SerializeField]
    Transform player;
    Vector3[] spawnPoint;
    float spawnTime;
    private int spawnIndex;
   // bool isPaused = false;
    Transform poolRoot;
    [SerializeField]
    SpawnData data;
    public void InitData()
    {
        key = data.key;
        spawnPoint= data.spawnPoint;
        key.prewarmCount = data.spawnPoint.Length;
        spawnTime= data.spawnTime;
    }

    private void Awake()
    {
        InitData();

    }
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.instance.BuildPool(key,transform);
        //StartCoroutine(SpawnCo());
        fireCo = StartCoroutine(SpawnCo());
    }

    public Vector3 GetSpawnPos(GameObject obj)
    {
        if (spawnPoint == null || spawnPoint.Length == 0)
        {
            Debug.Log("spawn Point°¡ ¾øÀ½");
            return transform.position;
        }
        SpawnSlot slot = obj.GetComponent<SpawnSlot>();
        if (slot == null)
        {
            slot = obj.AddComponent<SpawnSlot>();
        }
        if (slot.index < 0)
        {
            slot.index = spawnIndex % spawnPoint.Length;
            spawnIndex++;
        }
        return spawnPoint[slot.index];
    }
    public IEnumerator SpawnCo()
    {
            yield return new WaitForSeconds(spawnTime);
            Spawn();
            fireCo = null;
         //   StopCoroutine(fireCo);
        /*while (true)
        {
        }
        
        while (true)
        {
            while (isPaused)
            {
                yield return null;
            }
            yield return new WaitForSeconds(spawnTime);
            Spawn();
            isPaused = true;

        }
        */
    }
    public void ReSpawn()
    {
        if (fireCo != null)
        {
            return;
        }
          fireCo= StartCoroutine(SpawnCo());

        //isPaused = false;
    }

    public Coroutine fireCo;

    /*
    public void Return(GameObject obj)
    {
        PoolManager.instance.ReturnPool(obj);
    }
    */
    public void Spawn()
    {
       
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            GameObject obj = PoolManager.instance.UsePool(key);
            if (obj == null)
            {
                continue;
            }
            Vector3 pos = GetSpawnPos(obj);
            obj.transform.position = pos;
            Monster monster = obj.GetComponent<Monster>();
            if (monster != null)
            {
                monster.GoHome(pos);
                monster.SetTarget(player);
                monster.ondie -= ReSpawn;
                monster.ondie += ReSpawn;
              //  monster.onReturn -= Return;
              //  monster.onReturn += Return;
            }
            PoolManager.instance.ActivePool(obj);

        }

    }
}
