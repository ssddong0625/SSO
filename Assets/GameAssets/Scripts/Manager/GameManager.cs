using GameAssets.Scripts.Players;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace GameAssets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        [Header("매니저")]
        [SerializeField]
        private UiManager uiManager;
        [SerializeField]
        private SoundManager soundManager;
        [SerializeField]
        private CameraManager cameraManager;
        [SerializeField]
        private Player player;
        
        private PlayerSkill skill = new PlayerSkill();
        private PlayerStatus playerStatus = new PlayerStatus();




        public Player Player => player;
        public PlayerStatus PlayerStauts => playerStatus;
        public PlayerSkill Skill => skill;
        public UiManager UiManager => uiManager;
        public CameraManager CameraManager => cameraManager;
        public SoundManager SoundManager => soundManager;



       // [SerializeField]
     //   float exp=0;
      //  [SerializeField]
     //   int level=1;
       // float levelUpExp=100;
     //   float expStep=1.15f;
      //  public event Action onExpChanged;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            playerStatus.onLevelUp -= HandleLevelUp;
            playerStatus.onLevelUp += HandleLevelUp;
            playerStatus.onDie -= EndScene;
            playerStatus.onDie += EndScene;
            //exp = 0;
            //level = 1;
            //levelUpExp = 100;
            //expStep = 1.15f;
        }

        private void HandleLevelUp()
        {
            if(uiManager != null)
            {
                uiManager.UpdateLevel();
            }
            else
            {
                Debug.Log("UiManager 확인 ");
            }
        }
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        private void EndScene()
        {
            LoadScene(2);
            playerStatus.Reset();
        
        }

        
        
        
       // public float Exp
       // {
       //     get { return exp; }
       //     set
       //     {
       //         exp = value;
       //     }
       // }
       // public int Level
       // {
       //     get { return level; }
       // }
       // 
       // public int NeedExp()
       // {
       //     return Mathf.CeilToInt(levelUpExp * Mathf.Pow(expStep, level - 1));
       // }
       // public void AddExp(int amount)
       // {
       //     exp += amount;
       //     while (exp >= NeedExp())
       //     {
       //         exp -= NeedExp();
       //         level++;
       //         UiManager.instance.UpdateLevel();
       //     }
       //
       //     onExpChanged?.Invoke();
       // }

    }
}
