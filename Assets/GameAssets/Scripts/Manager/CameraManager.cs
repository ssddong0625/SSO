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
        //public float zoomSpeed = 0.5f;
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
           // TryGetComponent(out thirdPerson);
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
/*
        public void ZoomIn()
        {
            
            float y = Input.GetAxis("Mouse ScrollWheel");
            
        }
*/

        public void Update()
        {
            SwapCamera();
        }
    }

}
