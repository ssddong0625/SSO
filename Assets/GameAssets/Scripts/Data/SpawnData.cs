using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SpawnerData/SpanwerData",order =5)]
public class SpawnData : ScriptableObject
{
    public PoolKey key;
    public Vector3[] spawnPoint;
    public float spawnTime;
 
}
