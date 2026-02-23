using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Arrow : MonoBehaviour,IPoolable
{
    [SerializeField]
    LayerMask hitLayerMask;
    int atk;
    float speed;
    [SerializeField]
    Vector3 firePoint;
    Rigidbody rb;


    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 dir,float speed,int atk)
    {
        firePoint = dir;
        this.atk = atk;
        this.speed = speed;
    }
    public void OnDeSpawned()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnSpawned()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = firePoint * speed;
      //  rb.AddRelativeForce(firePoint * speed, ForceMode.Impulse);
    }

    IEnumerator ReturnCo()
    {
        yield return new WaitForSeconds(3f);
        PoolManager.instance.ReturnPool(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (((1 << other.gameObject.layer) & hitLayerMask.value) == 0) { return; }
        IHitAble hit = other.GetComponent<IHitAble>();
        Debug.Log(" µÇ³ª ?");
       
        hit?.Hit(atk);
        PoolManager.instance.ReturnPool(gameObject);
        Debug.Log(other.gameObject.name);


    }
}
*/