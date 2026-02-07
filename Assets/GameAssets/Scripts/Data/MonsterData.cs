using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAssets.Scripts.Data
{
    [CreateAssetMenu(menuName = "MonsterData/MonsterData", order = 1)]
    public class MonsterData : ScriptableObject
    {
        public int atk;
        public int exp;
        public float hp;
        public float maxHp;
        public GameObject dropItem;
    }
}

