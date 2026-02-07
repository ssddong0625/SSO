using UnityEngine;

public class TestPlayerHitBox : MonoBehaviour
{
    [SerializeField] private Transform hitBoxPivot;   // Player 자식(빈 오브젝트)
    [SerializeField] private float pitchClamp = 35f;  // 너무 꺾이지 않게 제한
    [SerializeField] private bool followYaw = true;   // 좌우도 카메라 따라갈지

    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // 공격 시작 프레임(애니메이션 이벤트)에서 호출 추천
    public void AlignHitBoxToCamera()
    {
        Vector3 camForward = cam.forward;

        // (1) 좌우(yaw)
        Vector3 flat = new Vector3(camForward.x, 0f, camForward.z);
        if (flat.sqrMagnitude < 0.0001f) flat = transform.forward;
        flat.Normalize();

        Quaternion yawRot = followYaw
            ? Quaternion.LookRotation(flat, Vector3.up)
            : Quaternion.LookRotation(transform.forward, Vector3.up);

        // (2) 상하(pitch)만 따로 계산해서 제한
        float pitch = Mathf.Asin(camForward.y) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        // (3) hitBoxPivot만 회전 (Player는 절대 건드리지 않음)
        hitBoxPivot.rotation = yawRot * Quaternion.Euler(pitch, 0f, 0f);
    }
}
