using UnityEngine;

public class WorldMapBoundsHelper : MonoBehaviour
{
    public Vector2 worldMin; // (minX, minZ)
    public Vector2 worldMax; // (maxX, maxZ)

    [ContextMenu("Auto Set Bounds From Terrains (Correct)")]
    public void AutoSetBoundsFromTerrains()
    {
        var terrains = FindObjectsOfType<Terrain>();
        if (terrains == null || terrains.Length == 0)
        {
            Debug.LogWarning("Terrain이 없습니다.");
            return;
        }

        float minX = float.PositiveInfinity;
        float minZ = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float maxZ = float.NegativeInfinity;

        foreach (var t in terrains)
        {
            // ? Terrain position = 좌하단(시작점)
            Vector3 p = t.transform.position;
            // ? Terrain size
            Vector3 s = t.terrainData.size;

            minX = Mathf.Min(minX, p.x);
            minZ = Mathf.Min(minZ, p.z);

            maxX = Mathf.Max(maxX, p.x + s.x);
            maxZ = Mathf.Max(maxZ, p.z + s.z);
        }

        worldMin = new Vector2(minX, minZ);
        worldMax = new Vector2(maxX, maxZ);

        Debug.Log($"[AutoBounds] worldMin={worldMin}, worldMax={worldMax}");
    }
}
