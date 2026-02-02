using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public WeaponData data;
        int atk;

        private void Awake()
        {
            InitData();
        }
        public void InitData()
        {
            atk = data.atk;
        }
        public void OnTriggerEnter(Collider other)
        {
            IHitAble hit = other.GetComponent<IHitAble>();
            hit?.Hit(atk);
        }
    }

}

