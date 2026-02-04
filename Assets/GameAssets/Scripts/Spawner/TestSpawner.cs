using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [SerializeField]
    private MonsterSpawnData spawnData;
    [SerializeField]
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        PoolManagerTest.instance.BuildPool(spawnData);
        StartCoroutine(SpawnCo());
    }

    public IEnumerator SpawnCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnData.monsterSpawnTime);
            GameObject obj = PoolManagerTest.instance.UsePool(spawnData);
            Monster monster = obj.GetComponent<Monster>();
            monster.Spawn();
            monster.SetTarget(player);
        }
    }
    public void Return(GameObject obj)
    {
        PoolManagerTest.instance.ReturnPool(spawnData,obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
