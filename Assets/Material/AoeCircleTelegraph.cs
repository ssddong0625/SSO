using UnityEngine;

public class AoeCircleTelegraph : MonoBehaviour
{
    [Header("Telegraph")]
    [SerializeField] private Transform visualRoot;          // Quad(원형 이미지)가 달린 Transform
    [SerializeField] private float telegraphTime = 1.2f;    // 예고 시간
    [SerializeField] private float radius = 3.0f;           // 최종 반지름(m)

    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask targetLayer;         // Player 레이어만 지정 추천

    [Header("VFX (Optional)")]
    [SerializeField] private GameObject explosionVfxPrefab; // 폭발 파티클(있으면)
    [SerializeField] private float destroyDelay = 0.2f;      // 폭발 후 장판 제거까지 딜레이

    [Header("Visual Pulse (Optional)")]
    [SerializeField] private Renderer ringRenderer;         // Quad의 Renderer
    [SerializeField] private string colorProperty = "_BaseColor"; // URP Unlit은 _BaseColor (기본)
    [SerializeField] private bool enablePulse = true;
    [SerializeField] private float pulseStartNormalized = 0.75f;  // 끝나기 75%부터 깜빡
    [SerializeField] private float pulseSpeed = 12f;              // 깜빡 속도
    [SerializeField] private float alphaMin = 0.35f;
    [SerializeField] private float alphaMax = 0.9f;

    private MaterialPropertyBlock mpb;
    private Vector3 startScale;
    private Vector3 endScale;
    private float elapsed;
    private bool fired;

    private void Awake()
    {
        if (visualRoot == null) visualRoot = transform;

        // Quad는 보통 1x1 사이즈라서, "반지름 r" 원을 만들려면 지름(2r)만큼 scale을 키우면 됩니다.
        // 즉 최종 scale = (2r, 2r, 1)
        startScale = new Vector3(0.01f, 0.01f, 1f);
        endScale = new Vector3(radius * 2f, radius * 2f, 1f);

        if (ringRenderer != null)
        {
            mpb = new MaterialPropertyBlock();
        }
    }

    private void OnEnable()
    {
        elapsed = 0f;
        fired = false;

        // 시작 스케일 적용
        visualRoot.localScale = startScale;
        SetAlpha(alphaMin);
    }

    private void Update()
    {
        // 1) 텔레그래프 진행
        elapsed += Time.deltaTime;
        float t = 0f;

        if (telegraphTime > 0f) t = Mathf.Clamp01(elapsed / telegraphTime);
        else t = 1f;

        // 2) 원형 장판 스케일 업 (부드럽게)
        // 개발자면 Ease를 쓰는 게 보기 좋아요.
        float eased = EaseOutCubic(t);
        visualRoot.localScale = Vector3.LerpUnclamped(startScale, endScale, eased);

        // 3) 끝나기 직전에 깜빡임(선택)
        if (enablePulse && ringRenderer != null && t >= pulseStartNormalized)
        {
            float pulse = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed));
            float a = Mathf.Lerp(alphaMin, alphaMax, pulse);
            SetAlpha(a);
        }

        // 4) 텔레그래프 종료 → 폭발(판정 1회)
        if (!fired && t >= 1f)
        {
            fired = true;
            ExplodeOnce();
        }
    }

    private void ExplodeOnce()
    {
        // 폭발 VFX
        if (explosionVfxPrefab != null)
        {
            Instantiate(explosionVfxPrefab, transform.position, Quaternion.identity);
        }

        // 데미지 판정(원형 범위)
        // OverlapSphere로 targetLayer만 가져오고, IDamageable이면 데미지
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hits.Length; i++)
        {
            IHitAble d = hits[i].GetComponentInParent<IHitAble>();
            if (d != null)
            {
                d.Hit(damage);
            }
        }

        // 정리
        Destroy(gameObject, destroyDelay);
    }

    private void SetAlpha(float a)
    {
        if (ringRenderer == null) return;

        if (mpb == null) mpb = new MaterialPropertyBlock();
        ringRenderer.GetPropertyBlock(mpb);

        // _BaseColor가 없는 셰이더면 _Color일 수도 있으니 필요하면 바꾸세요.
        Color c = Color.red;
        c.a = a;

        mpb.SetColor(colorProperty, c);
        ringRenderer.SetPropertyBlock(mpb);
        
    }
    
    private float EaseOutCubic(float x)
    {
        // 0~1 입력을 부드럽게(초반 빠르고 끝에서 감속)
        float inv = 1f - x;
        return 1f - inv * inv * inv;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 씬 뷰에서 반지름 확인용
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
