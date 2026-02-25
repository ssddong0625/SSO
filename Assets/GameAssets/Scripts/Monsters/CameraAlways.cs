using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlways : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void Awake()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
    }
    private void LateUpdate()
    {
        if (cam == null) { return; }
        transform.forward = cam.transform.forward;
    }
 
}
