using System.Collections.Generic;
using UnityEngine;

public sealed class PlacementService
{
    // placePrefab 기준 bounds 캐시 (프리뷰와 무관)
    private readonly Dictionary<GameObject, Bounds> _prefabLocalBoundsCache = new();
    private GameObject _currentPrefab;
    private Bounds _bounds;   // placePrefab 기준(루트 로컬)
    private bool _hasBounds;

    public void SetReferencePrefab(GameObject placePrefab)
    {
        if (placePrefab == null)
        {
            _currentPrefab = null;
            _hasBounds = false;
            return;
        }

        if (_currentPrefab == placePrefab && _hasBounds) return;

        _currentPrefab = placePrefab;

        if (_prefabLocalBoundsCache.TryGetValue(placePrefab, out var cached))
        {
            _bounds = cached;
            _hasBounds = true;
            return;
        }

        // 임시 인스턴스 생성(정확한 bounds 산출) → 1회만, 이후 캐시
        var temp = Object.Instantiate(placePrefab);
        temp.transform.position = Vector3.zero;
        temp.transform.rotation = Quaternion.identity;
        temp.transform.localScale = Vector3.one;

        Bounds b;
        bool has = false;

        var renderers = temp.GetComponentsInChildren<Renderer>(true);
        if (renderers != null && renderers.Length > 0)
        {
            b = ToRootLocalBounds(temp.transform, renderers[0].transform, renderers[0].localBounds);
            has = true;
            for (int i = 1; i < renderers.Length; i++)
                b.Encapsulate(ToRootLocalBounds(temp.transform, renderers[i].transform, renderers[i].localBounds));
        }
        else
        {
            var cols = temp.GetComponentsInChildren<Collider>(true);
            if (cols != null && cols.Length > 0)
            {
                // Collider는 world bounds라 root local로 변환
                b = WorldAabbToRootLocalBounds(temp.transform, cols[0].bounds);
                has = true;
                for (int i = 1; i < cols.Length; i++)
                    b.Encapsulate(WorldAabbToRootLocalBounds(temp.transform, cols[i].bounds));
            }
            else
            {
                has = false;
                b = new Bounds(Vector3.zero, Vector3.one);
            }
        }

        Object.Destroy(temp);

        _bounds = b;
        _hasBounds = has;
        _prefabLocalBoundsCache[placePrefab] = b;
    }

    //멀티포인트 groundY 계산(기준은 placePrefab bounds)
    public float ComputeGroundY_MultiPoint(BuildingData data, Vector3 pivotPosXZ)
    {
        if (data == null || !_hasBounds) return pivotPosXZ.y;

        Vector3 c = _bounds.center;
        Vector3 e = _bounds.extents;
        float bottomY = c.y - e.y;

        Vector3[] points =
        {
            new Vector3(c.x - e.x, bottomY, c.z - e.z),
            new Vector3(c.x - e.x, bottomY, c.z + e.z),
            new Vector3(c.x + e.x, bottomY, c.z - e.z),
            new Vector3(c.x + e.x, bottomY, c.z + e.z),
            new Vector3(c.x,       bottomY, c.z)
        };

        float maxY = float.NegativeInfinity;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 wp = pivotPosXZ + points[i];
            Vector3 rayStart = wp + Vector3.up * 1.0f;

            if (Physics.Raycast(rayStart, Vector3.down, out var hit, 5f, data.groundLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.y > maxY) maxY = hit.point.y;
            }
        }

        if (maxY == float.NegativeInfinity) return pivotPosXZ.y;
        return maxY;
    }

    //“pivot이 어디에 있어야 바닥에 닿는지”를 placePrefab bounds로 계산
    public Vector3 GetPivotPosSnappedToGround(Vector3 pivotPosXZ, float groundY)
    {
        if (!_hasBounds) return pivotPosXZ;

        float pivotToBottom = -(_bounds.center.y - _bounds.extents.y); // = -bounds.min.y
        pivotPosXZ.y = groundY + pivotToBottom;
        return pivotPosXZ;
    }

    public bool CanPlace(BuildingData data, Vector3 pivotPos, Vector3 groundNormal, out string reason)
    {
        reason = "";

        if (data == null) { reason = "BuildingData 없음"; return false; }
        if (!_hasBounds) { reason = "placePrefab Bounds 캐시 없음"; return false; }

        // 경사도
        float slope = Vector3.Angle(groundNormal, Vector3.up);
        if (slope > data.maxSlopeAngle)
        {
            reason = $"경사도 초과 ({slope:0.#}°)";
            return false;
        }

        // 겹침
        Vector3 center = pivotPos + _bounds.center;
        Vector3 half = _bounds.extents;

        float eps = Mathf.Max(0f, data.overlapEpsilon);
        half = new Vector3(
            Mathf.Max(0.01f, half.x - eps),
            Mathf.Max(0.01f, half.y - eps),
            Mathf.Max(0.01f, half.z - eps)
        );

        bool blocked = Physics.CheckBox(center, half, Quaternion.identity, data.blockedLayer, QueryTriggerInteraction.Ignore);
        if (blocked)
        {
            if (data.verboseBlockReason)
            {
                var hits = Physics.OverlapBox(center, half, Quaternion.identity, data.blockedLayer, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < hits.Length; i++)
                {
                    var h = hits[i];
                    if (h == null) continue;

                    int hMask = 1 << h.gameObject.layer;
                    if ((hMask & data.groundLayer) != 0) continue;

                    reason = $"겹침: {h.name} (Layer:{LayerMask.LayerToName(h.gameObject.layer)})";
                    return false;
                }
            }
            reason = "겹침";
            return false;
        }

        // 접지(옵션)
        if (data.useMultiPointGroundCheck)
        {
            if (!MultiPointGroundGapCheck(data, pivotPos, out reason))
                return false;
        }

        reason = "설치 가능";
        return true;
    }

    private bool MultiPointGroundGapCheck(BuildingData data, Vector3 pivotPos, out string reason)
    {
        Vector3 c = _bounds.center;
        Vector3 e = _bounds.extents;
        float bottomY = c.y - e.y;

        Vector3[] points =
        {
            new Vector3(c.x - e.x, bottomY, c.z - e.z),
            new Vector3(c.x - e.x, bottomY, c.z + e.z),
            new Vector3(c.x + e.x, bottomY, c.z - e.z),
            new Vector3(c.x + e.x, bottomY, c.z + e.z),
            new Vector3(c.x,       bottomY, c.z)
        };

        float maxGap = 0f;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 wp = pivotPos + points[i];
            Vector3 rayStart = wp + Vector3.up * 0.5f;

            if (!Physics.Raycast(rayStart, Vector3.down, out var hit, 2f, data.groundLayer, QueryTriggerInteraction.Ignore))
            {
                reason = "바닥 접지 실패";
                return false;
            }

            float gap = (rayStart.y - hit.point.y) - 0.5f;
            if (gap > maxGap) maxGap = gap;
        }

        if (maxGap > data.maxGroundGap)
        {
            reason = $"지면과 간격 큼 (gap {maxGap:0.###}m)";
            return false;
        }

        reason = "";
        return true;
    }

    private Bounds ToRootLocalBounds(Transform root, Transform child, Bounds childLocal)
    {
        Vector3 c = childLocal.center;
        Vector3 e = childLocal.extents;

        Vector3[] corners =
        {
            c + new Vector3(+e.x,+e.y,+e.z), c + new Vector3(+e.x,+e.y,-e.z),
            c + new Vector3(+e.x,-e.y,+e.z), c + new Vector3(+e.x,-e.y,-e.z),
            c + new Vector3(-e.x,+e.y,+e.z), c + new Vector3(-e.x,+e.y,-e.z),
            c + new Vector3(-e.x,-e.y,+e.z), c + new Vector3(-e.x,-e.y,-e.z),
        };

        Vector3 first = root.InverseTransformPoint(child.TransformPoint(corners[0]));
        Bounds b = new Bounds(first, Vector3.zero);
        for (int i = 1; i < corners.Length; i++)
            b.Encapsulate(root.InverseTransformPoint(child.TransformPoint(corners[i])));
        return b;
    }

    private Bounds WorldAabbToRootLocalBounds(Transform root, Bounds worldAabb)
    {
        Vector3 c = worldAabb.center;
        Vector3 e = worldAabb.extents;

        Vector3[] corners =
        {
            c + new Vector3(+e.x,+e.y,+e.z), c + new Vector3(+e.x,+e.y,-e.z),
            c + new Vector3(+e.x,-e.y,+e.z), c + new Vector3(+e.x,-e.y,-e.z),
            c + new Vector3(-e.x,+e.y,+e.z), c + new Vector3(-e.x,+e.y,-e.z),
            c + new Vector3(-e.x,-e.y,+e.z), c + new Vector3(-e.x,-e.y,-e.z),
        };

        Vector3 first = root.InverseTransformPoint(corners[0]);
        Bounds b = new Bounds(first, Vector3.zero);
        for (int i = 1; i < corners.Length; i++)
            b.Encapsulate(root.InverseTransformPoint(corners[i]));
        return b;
    }
}