using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlayerArrow : MonoBehaviour
{
    [SerializeField]
    RectTransform arrowUi;
    [SerializeField]
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!arrowUi || !player) { return; }
        float yaw = player.eulerAngles.y;
        arrowUi.localEulerAngles = new Vector3(0f, 0f, -yaw);
    }
}
