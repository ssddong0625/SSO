using UnityEngine;

public class ArrowProjectile : MonoBehaviour, IPoolable
{
    private Vector3 targetPos;
    private float speed = 40f;

    public void Init(Vector3 start, Vector3 target)
    {
        transform.position = start;
        targetPos = target;

        Vector3 dir = (target - start).normalized;
        transform.forward = dir;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            PoolManager.instance.ReturnPool(gameObject);
        }
    }

    public void OnSpawned() { }
    public void OnDeSpawned() { }
}