using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterSpawnSo/MonsterSpanwSo",order =3)]
public class MonsterSpawnData : ScriptableObject
{
    public int poolCount;
    public GameObject targetObj;
    public Monster monster;
    public int monsterSpawnTime;
    public Vector3[] monsterSpawn;
    
}
