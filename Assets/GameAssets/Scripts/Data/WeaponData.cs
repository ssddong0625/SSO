using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Scripts.Data
{
    public enum WeaponType
    {
        Sword,
        Knuckle,
        Bow
    }
    [CreateAssetMenu(menuName = "Weapon/WeaponData", order = 2)]
    public class WeaponData : ScriptableObject
    {
        public int atk;
        public WeaponType type;
        public GameObject prefab;

    }

}

