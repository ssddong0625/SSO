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
        private UIToggle uitoggle;
        [SerializeField]
        private SoundManager soundManager;
        [SerializeField]
        private CameraManager cameraManager;
        [SerializeField]
        private Player player;
        [SerializeField]
        private PlayerEquip playerEquip;
        [SerializeField]
        private Bow bow;
        private PlayerStatus playerStatus = new PlayerStatus();

        public Player Player => player;
        public PlayerEquip PlayerEquip => playerEquip;
        public PlayerStatus PlayerStauts => playerStatus;
        
        public Bow Bow => bow;
        public UiManager UiManager => uiManager;
        public CameraManager CameraManager => cameraManager;
        public SoundManager SoundManager => soundManager;
        public UIToggle UIToggle => uitoggle;



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
          
        }

        private void HandleLevelUp()
        {
            if(uiManager != null)
            {
                uiManager.UpdateLevel();
                uiManager.PlayerRefreshHpUiText();
                uiManager.PlayerRefreshHpUiImg();
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
            LoadScene(1);
            playerStatus.Reset();
        
        }

    
    }
}
