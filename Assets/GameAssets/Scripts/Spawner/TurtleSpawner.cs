using GameAssets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleSpawner : MonoBehaviour
{
    public MonsterSpawnData data;
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.instance.InitData(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
