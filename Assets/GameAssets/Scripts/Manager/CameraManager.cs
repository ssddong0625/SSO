using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace GameAssets.Scripts.Manager
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineVirtualCamera thirdPerson;
        public CinemachineVirtualCamera onePerson;

        public int active = 20;
        
        public int inactive = 10;

        public void SwapCamera()
        {
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (thirdPerson.Priority > onePerson.Priority)
                {
                    OnePerson();
                }
                else
                {
                    ThirdPerson();
                }
            }
        }
        public void Start()
        {
            ThirdPerson();
        }
        public void ThirdPerson()
        {
            thirdPerson.Priority = active;
            onePerson.Priority = inactive;
        }
        public void OnePerson()
        {
            onePerson.Priority = active;
            thirdPerson.Priority = inactive;
        }

        public void Update()
        {
            SwapCamera();
        }
    }

}
