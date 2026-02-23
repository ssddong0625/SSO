using UnityEngine;

public class PreviewProvider : MonoBehaviour
{
    [SerializeField] private Transform previewParent;

    public GameObject Preview { get; private set; }
    public Renderer[] Renderers { get; private set; }
    public Collider[] Colliders { get; private set; }
    public PoolKey CurrentKey { get; private set; }

    private MaterialPropertyBlock _mpb;
    private static readonly int ColorId = Shader.PropertyToID("_Color");
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor"); // URP 대비

    private void Awake()
    {
        if (previewParent == null)
        {
            var go = new GameObject("[PreviewParent]");
            previewParent = go.transform;
        }
        _mpb = new MaterialPropertyBlock();
    }

    public bool EnsurePreview(PoolKey key)
    {
        if (PoolManager.instance == null) return false;
        if (key == null) return false;

        if (Preview != null && CurrentKey == key) return false;

        ReleasePreview();
        CurrentKey = key;

        
        PoolManager.instance.BuildPool(CurrentKey, previewParent);
        Preview = PoolManager.instance.UsePool(CurrentKey);
        if (Preview == null)
        {
            Debug.LogWarning("프리뷰 풀에서 오브젝트를 못 가져왔습니다. PoolKey.prewarmCount(>=1) 확인");
            CurrentKey = null;
            return false;
        }
        Preview.transform.SetParent(previewParent, false);
        PoolManager.instance.ActivePool(Preview);

        // 프리뷰는 판정에 끼지 않게 콜라이더 OFF
        Colliders = Preview.GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < Colliders.Length; i++)
            Colliders[i].enabled = false;

        Renderers = Preview.GetComponentsInChildren<Renderer>(true);

        // 레이캐스트 영향 제거(선택이지만 추천)
        int ignore = LayerMask.NameToLayer("Ignore Raycast");
        if (ignore >= 0) SetLayerRecursively(Preview, ignore);

        SetPreviewAlpha(1f);
        return true;
    }

    public void ReleasePreview()
    {
        if (Preview == null) return;

        PoolManager.instance.ReturnPool(Preview);

        Preview = null;
        Renderers = null;
        Colliders = null;
        CurrentKey = null;
    }

    public void ApplyMaterial(Material mat)
    {
        if (Renderers == null || mat == null) return;
        for (int i = 0; i < Renderers.Length; i++)
            Renderers[i].material = mat;
    }

    public void SetPreviewAlpha(float alpha01)
    {
        if (Renderers == null) return;
        alpha01 = Mathf.Clamp01(alpha01);

        for (int i = 0; i < Renderers.Length; i++)
        {
            var r = Renderers[i];
            if (r == null) continue;

            r.GetPropertyBlock(_mpb);

            // Standard/URP 둘 다 대응
            Color c = Color.white;
            if (r.sharedMaterial != null)
            {
                if (r.sharedMaterial.HasProperty(BaseColorId)) c = r.sharedMaterial.GetColor(BaseColorId);
                else if (r.sharedMaterial.HasProperty(ColorId)) c = r.sharedMaterial.GetColor(ColorId);
            }

            c.a = alpha01;

            if (r.sharedMaterial != null && r.sharedMaterial.HasProperty(BaseColorId)) _mpb.SetColor(BaseColorId, c);
            else _mpb.SetColor(ColorId, c);

            r.SetPropertyBlock(_mpb);
        }
    }

    private void SetLayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform t in go.transform)
            SetLayerRecursively(t.gameObject, layer);
    }
}