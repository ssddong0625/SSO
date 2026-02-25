using System.Collections;
using UnityEngine;

public class ArrowVfxPooled : MonoBehaviour, IPoolable
{
    [Header("Move")]
    [SerializeField] private float speed = 35f;
    [SerializeField] private float maxLifeTime = 2.5f;
    private ParticleSystem ps;
    private Coroutine moveCo;

    // Bow에서 호출
    public void Fire(Vector3 start, Vector3 end)
    {
        transform.position = start;

        Vector3 dir = end - start;
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir.normalized);

        if (moveCo != null) StopCoroutine(moveCo);
        moveCo = StartCoroutine(MoveRoutine(end));
    }
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>(); // 루트에 파티클이면 이걸로 OK
        // 루트가 아니고 자식이면: ps = GetComponentInChildren<ParticleSystem>();
    }

    private IEnumerator MoveRoutine(Vector3 end)
    {
        float t = 0f;

        while (t < maxLifeTime)
        {
            t += Time.deltaTime;

            Vector3 to = end - transform.position;
            float step = speed * Time.deltaTime;

            if (to.magnitude <= step)
            {
                transform.position = end;
                break;
            }

            Vector3 dir = to.normalized;
            transform.position += dir * step;
            //transform.rotation = Quaternion.LookRotation(dir);

            yield return null;
        }

        // 풀 반납
        if (PoolManager.instance != null)
            PoolManager.instance.ReturnPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    // ===== IPoolable =====
    public void OnSpawned()
    {
        // 트레일/파티클 초기화가 필요하면 여기서
    }

    public void OnDeSpawned()
    {
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
    }
}