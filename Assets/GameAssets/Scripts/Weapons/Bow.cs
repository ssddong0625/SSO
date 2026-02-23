using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bow : Weapon
{
    [SerializeField]
    PoolKey key;
    //int atk;
    [SerializeField]
    Transform firePoint;
    [SerializeField]
    LayerMask layerMask;
    
    private void OnEnable()
    {
        
    }

    public void BowChange()
    {
        PoolManager.instance.BuildPool(key, transform);
    }
    // public override void Attack()
    // {
    //     GameObject obj= PoolManager.instance.UsePool(key);
    //
    //     var arrow = obj.GetComponent<Arrow>();
    //     arrow.Fire(firePoint.forward,data.atk,data.atk);
    //
    //
    //     PoolManager.instance.ActivePool(obj);
    //     obj.transform.position = firePoint.position;    
    //     //obj.transform.rotation = firePoint.rotation;
    //
    //
    // }
    public override void Attack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            Monster mon = hit.collider.GetComponentInParent<Monster>();
            IHitAble hits = hit.collider.GetComponent<IHitAble>();
            if (hits != null)
            {
                Debug.Log(hits);
                mon.SetAttacker(transform);
                hits.Hit(data.atk);
                
            }
        }

    }
    public  void Skill()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            Monster mon = hit.collider.GetComponentInParent<Monster>();
            IHitAble hits = hit.collider.GetComponent<IHitAble>();
            if (hits != null)
            {
                Debug.Log(hits);
                mon.SetAttacker(transform);
                hits.Hit(10*data.atk);

            }
        }
    }
}

