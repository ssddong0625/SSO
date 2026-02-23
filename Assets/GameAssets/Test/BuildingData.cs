using UnityEngine;

[CreateAssetMenu(menuName = "Build/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildingName;

    [Header("실제 설치될 프리팹")]
    public GameObject placePrefab;

    [Header("프리뷰(고스트) 풀 키")]
    public PoolKey previewPoolKey;

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask blockedLayer; //Ground 레이어 넣지 마세요(항상 겹침 뜸)

    [Header("Grid Snap")]
    public bool useGridSnap = true;
    public float gridSize = 1f;

    [Header("Rules")]
    [Range(0f, 89f)] public float maxSlopeAngle = 25f;
    public float maxGroundGap = 0.08f;
    public bool useMultiPointGroundCheck = true;

    [Header("Overlap")]
    [Range(0f, 0.1f)] public float overlapEpsilon = 0.02f;
    public bool verboseBlockReason = true;

    [Header("Build Range")]
    public float maxBuildDistance = 8f; // 건물별 설치 거리(0이면 컨트롤러 기본값 사용)
}