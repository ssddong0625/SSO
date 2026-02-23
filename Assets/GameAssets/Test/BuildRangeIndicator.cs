using UnityEngine;

public class BuildRangeIndicator : MonoBehaviour
{
    [SerializeField] private Transform ringVisual;
    [SerializeField] private float yOffset = 0.02f;

    public void SetVisible(bool visible)
    {
        if (ringVisual != null) ringVisual.gameObject.SetActive(visible);
    }

    public void SetRadius(float radius)
    {
        radius = Mathf.Max(0.1f, radius);
        if (ringVisual == null) return;

        float diameter = radius * 2f;
        ringVisual.localScale = new Vector3(diameter, ringVisual.localScale.y, diameter);
    }

    public void Follow(Transform target)
    {
        if (ringVisual == null || target == null) return;

        Vector3 p = target.position;
        p.y += yOffset;
        ringVisual.position = p;
    }
}