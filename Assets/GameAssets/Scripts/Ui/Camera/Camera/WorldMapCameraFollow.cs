using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class WorldMapCameraFollow : MonoBehaviour
{
    [SerializeField]
    RectTransform mapRect;
    [SerializeField]
    RectTransform playerMarker;
    public Transform player;
    public Vector2 worldMin;
    public Vector2 worldMax;

    
    public float roattionOffSet;
    public bool flipy = false;
    public float height;
    public float yoffset;

    // Start is called before the first frame update
    void Start()
    {
        roattionOffSet = 0f;

    }

    public void PlayerMarker()
    {
        /*
        float nx = Mathf.InverseLerp(worldMin.x, worldMax.x, player.position.x);
        float nz = Mathf.InverseLerp(worldMin.y,worldMax.y, player.position.z);

        nx = Mathf.Clamp01(nx);
        nz = Mathf.Clamp01(nz);
        */
        float minX = Mathf.Min(worldMin.x, worldMax.x);
        float maxX = Mathf.Max(worldMin.x, worldMax.x);
        float minZ = Mathf.Min(worldMin.y, worldMax.y);
        float maxZ = Mathf.Max(worldMin.y, worldMax.y);

        float nx = Mathf.InverseLerp(minX, maxX, player.position.x);
        float nz = Mathf.InverseLerp(minZ, maxZ, player.position.z);
        if (flipy)
        {
            nz = 1f - nz;
        }

        Vector2 size = mapRect.rect.size;
        Vector2 localPos = new Vector2((nx - 0.5f) * size.x, (nz - 0.5f) * size.y);
        playerMarker.anchoredPosition =localPos;

        float yaw= player.eulerAngles.y;
        playerMarker.localEulerAngles = new Vector3(0f, 0f, -yaw + roattionOffSet);

    }

    public void LateUpdate()
    {
        PlayerMarker();
    }
















}

  
