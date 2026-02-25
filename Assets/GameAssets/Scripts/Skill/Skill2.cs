using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Skill2 : MonoBehaviour, IPoolable
{
    [SerializeField]
    float movespeed=500F;
    [SerializeField]
    float size;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    int atk=100;
    float time = 5f;
    //[SerializeField]
    // Transform player;

    Coroutine returnCo;
    public void OnDeSpawned()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnSpawned()
    {
        rb.velocity = transform.forward * movespeed * 10;
        if (returnCo != null) StopCoroutine(returnCo);
        returnCo = StartCoroutine(afterCo(time));
    }
    IEnumerator afterCo(float t)
    {
        
        yield return new WaitForSeconds(t);
        PoolManager.instance.ReturnPool(gameObject);
    }
    
    public void ShotFire()
    {

        rb.velocity = transform.forward * movespeed*10;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //PoolManager.instance.BuildPool(key);
        rb=GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) == 0) 
        {
            return;
        }
        Debug.Log("충돌 일어남");
        IHitAble hit = other.GetComponent<IHitAble>();
        hit.Hit(atk);
        PoolManager.instance.ReturnPool(gameObject);


    }
    // Update is called once per frame
    void Update()
    {
      //  ShotFire();
    }
}
