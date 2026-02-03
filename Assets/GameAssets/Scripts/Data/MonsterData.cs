using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAssets.Scripts.Data
{
    [CreateAssetMenu(menuName = "Monster/MonsterData", order = 1)]
    public class MonsterData : ScriptableObject
    {
        public int atk;
        public int exp;
        public int hp;
       // public float attackSpeed;
        public GameObject monsterPrefab;
        public GameObject dropItem;
    }
}

