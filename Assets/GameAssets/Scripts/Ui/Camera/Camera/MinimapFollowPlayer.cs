using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollowPlayer : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    float height;
    [SerializeField]
    float yoffset;
    // Start is called before the first frame update
    void Start()
    {
        height = 30f;
        yoffset = 0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!player) return;
        Vector3 pos = player.position;
        pos.y += height;
        transform.position = pos;
        float yaw = player.eulerAngles.y + yoffset;
        transform.rotation = Quaternion.Euler(90f, yaw, 0f);
    }
}
