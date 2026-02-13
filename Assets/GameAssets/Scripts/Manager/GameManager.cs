using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GameAssets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        [SerializeField]
        float exp;
        [SerializeField]
        int level;
        float levelUpExp;
        float expStep;
        public event Action onExpChanged;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                DontDestroyOnLoad(gameObject);
            }
            exp = 0;
            level = 1;
            levelUpExp = 100;
            expStep = 1.15f;
        }
        public float Exp
        {
            get { return exp; }
            set
            {
                exp = value;
            }
        }
        public int Level
        {
            get { return level; }
        }

        public int NeedExp()
        {
            return Mathf.CeilToInt(levelUpExp * Mathf.Pow(expStep, level - 1));
        }
        public void AddExp(int amount)
        {
            exp += amount;
            while (exp >= NeedExp())
            {
                exp -= NeedExp();
                level++;
                UiManager.instance.UpdateLevel();
            }

            onExpChanged?.Invoke();
        }

    }
}
