using System;
using UnityEngine;

public class BuildControllerSM : MonoBehaviour
{
    public enum BuildState { Off, PreviewingValid, PreviewingBlocked }

    [Header("Refs")]
    [SerializeField] private Camera cam;
    [SerializeField] private PreviewProvider previewProvider;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Range Indicator (Optional)")]
    [SerializeField] private BuildRangeIndicator rangeIndicator;

    [Header("Current Building")]
    [SerializeField] private BuildingData current;

    [Header("Preview Materials")]
    [SerializeField] private Material canPlaceMat;
    [SerializeField] private Material cannotPlaceMat;

    [Header("Keys")]
    [SerializeField] private KeyCode toggleKey = KeyCode.B;
    [SerializeField] private KeyCode cancelKey = KeyCode.Mouse1;

    [Header("Preview Y Smoothing")]
    [SerializeField] private float ySmoothTime = 0.06f;

    [Header("Build Range")]
    [SerializeField] private float defaultMaxBuildDistance = 8f;
    [SerializeField] private float outOfRangeAlpha = 0.35f;
    [SerializeField] private bool showOutOfRangeText = true;

    public BuildState State { get; private set; } = BuildState.Off;

    public event Action<BuildState> OnStateChanged;
    public event Action<string> OnReasonChanged;

    private readonly PlacementService placement = new PlacementService();

    private string lastReason = "";
    private float yVelocity;

    // 설치/판정용 확정 좌표(스무딩 X)
    private Vector3 finalPivotPos;

    private void Start()
    {
        if (cam == null) cam = Camera.main;
        if (previewProvider == null) previewProvider = GetComponent<PreviewProvider>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // current가 이미 잡혀있다면 placePrefab 기준 캐시
        if (current != null && current.placePrefab != null)
            placement.SetReferencePrefab(current.placePrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (State == BuildState.Off) EnterBuild();
            else ExitBuild();
        }

        if (State == BuildState.Off) return;

        if (Input.GetKeyDown(cancelKey))
        {
            ExitBuild();
            return;
        }

        if (rangeIndicator != null && player != null)
            rangeIndicator.Follow(player);

        TickBuild();
    }

    private void EnterBuild()
    {
        SetState(BuildState.PreviewingBlocked);
        PushReason("설치 위치를 선택하세요");
        UpdateRangeIndicator(true);
        PreparePreview();
    }

    private void ExitBuild()
    {
        previewProvider?.ReleasePreview();
        SetState(BuildState.Off);
        PushReason("");
        yVelocity = 0f;
        UpdateRangeIndicator(false);
    }

    private void TickBuild()
    {
        if (PoolManager.instance == null) return;
        if (cam == null || previewProvider == null) return;
        if (current == null || current.placePrefab == null || current.previewPoolKey == null) return;

        // 항상 placePrefab 기준으로 계산(재발 방지 핵심)
        placement.SetReferencePrefab(current.placePrefab);

        previewProvider.EnsurePreview(current.previewPoolKey);
        if (previewProvider.Preview == null) return;

        if (!TryGetMouseGround(out RaycastHit groundHit))
        {
            previewProvider.Preview.SetActive(false);
            SetState(BuildState.PreviewingBlocked);
            PushReason("바닥을 찾을 수 없음");
            return;
        }

        previewProvider.Preview.SetActive(true);

        Vector3 pivotPos = groundHit.point;

        if (current.useGridSnap)
            pivotPos = SnapToGridXZ(pivotPos, current.gridSize);

        // 거리 제한
        if (player == null)
        {
            previewProvider.ApplyMaterial(cannotPlaceMat);
            previewProvider.SetPreviewAlpha(outOfRangeAlpha);
            SetState(BuildState.PreviewingBlocked);
            PushReason("플레이어 Transform 연결 필요");
            previewProvider.Preview.transform.position = pivotPos;
            return;
        }

        float maxDist = (current.maxBuildDistance > 0f) ? current.maxBuildDistance : defaultMaxBuildDistance;
        bool inRange = (pivotPos - player.position).sqrMagnitude <= maxDist * maxDist;

        // 확정 groundY(멀티포인트) → 확정 pivotPosY(스무딩 X)
        float groundY = placement.ComputeGroundY_MultiPoint(current, pivotPos);
        finalPivotPos = placement.GetPivotPosSnappedToGround(pivotPos, groundY); // [NEW] 확정 좌표

        // 프리뷰 표시는 스무딩(Y만)
        Vector3 visualPos = finalPivotPos;
        float currentY = previewProvider.Preview.transform.position.y;
        visualPos.y = Mathf.SmoothDamp(currentY, finalPivotPos.y, ref yVelocity, ySmoothTime);
        previewProvider.Preview.transform.position = visualPos;

        // 범위 밖 처리(프리뷰는 보여주되 투명+빨강+텍스트)
        if (!inRange)
        {
            previewProvider.ApplyMaterial(cannotPlaceMat);
            previewProvider.SetPreviewAlpha(outOfRangeAlpha);

            SetState(BuildState.PreviewingBlocked);
            PushReason(showOutOfRangeText ? $"범위 밖: 최대 {maxDist:0.#}m" : "");
            return;
        }

        previewProvider.SetPreviewAlpha(1f);

        // 설치 판정은 “확정 좌표(finalPivotPos)”로만 수행 (스무딩 값 사용 금지)
        bool canPlace = placement.CanPlace(current, finalPivotPos, groundHit.normal, out string reason);

        if (canPlace)
        {
            SetState(BuildState.PreviewingValid);
            previewProvider.ApplyMaterial(canPlaceMat);
            PushReason("");
        }
        else
        {
            SetState(BuildState.PreviewingBlocked);
            previewProvider.ApplyMaterial(cannotPlaceMat);
            PushReason(reason);
        }

        // 설치도 “확정 좌표(finalPivotPos)”로만 수행 (재발 차단)
        if (Input.GetMouseButtonDown(0))
        {
            if (State == BuildState.PreviewingValid)
                Instantiate(current.placePrefab, finalPivotPos, Quaternion.identity);
        }
    }

    private void PreparePreview()
    {
        if (previewProvider == null) return;
        if (PoolManager.instance == null) return;
        if (current == null || current.previewPoolKey == null) return;

        previewProvider.EnsurePreview(current.previewPoolKey);
    }

    public void SetCurrentBuilding(BuildingData data)
    {
        current = data;

        if (current != null && current.placePrefab != null)
            placement.SetReferencePrefab(current.placePrefab); // 건물 바꿀 때 기준 프리팹 캐시

        if (State == BuildState.Off) return;

        previewProvider?.ReleasePreview();
        PreparePreview();
        SetState(BuildState.PreviewingBlocked);
        PushReason("설치 위치를 선택하세요");
        yVelocity = 0f;

        UpdateRangeIndicator(true);
    }

    private void UpdateRangeIndicator(bool visible)
    {
        if (rangeIndicator == null) return;

        rangeIndicator.SetVisible(visible);

        float maxDist = (current != null && current.maxBuildDistance > 0f)
            ? current.maxBuildDistance
            : defaultMaxBuildDistance;

        rangeIndicator.SetRadius(maxDist);

        if (player != null)
            rangeIndicator.Follow(player);
    }

    private void SetState(BuildState s)
    {
        if (State == s) return;
        State = s;
        OnStateChanged?.Invoke(State);
    }

    private void PushReason(string reason)
    {
        reason ??= "";
        if (reason == lastReason) return;
        lastReason = reason;
        OnReasonChanged?.Invoke(reason);
    }

    private bool TryGetMouseGround(out RaycastHit hit)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 1000f, current.groundLayer, QueryTriggerInteraction.Ignore);
    }

    private Vector3 SnapToGridXZ(Vector3 p, float grid)
    {
        p.x = Mathf.Round(p.x / grid) * grid;
        p.z = Mathf.Round(p.z / grid) * grid;
        return p;
    }
}