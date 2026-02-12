using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PoolKey/PoolKey",order =4)]
public class PoolKey : ScriptableObject
{
    [Header("프리펩 넣으시오")]
    public GameObject prefab;
    [Header("풀링 갯수")]
    public int prewarmCount;

       
}
