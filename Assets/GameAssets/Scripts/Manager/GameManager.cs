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
        int exp;
        [SerializeField]
        int level;
        int maxExp;
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
            maxExp = 100;
        }
        
        public int Exp
        {
            get { return exp; }
            set
            {
                exp = value;
                Debug.Log($"남은 경험치는 {exp} 입니다");
                if (exp >= maxExp)
                {
                    LevelUp();
                }
            }
        }
        public void LevelUp()
        {
            level++;
            maxExp = maxExp * 2;
            Debug.Log(maxExp);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
