using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossPattern : MonoBehaviour,IHitAble
{
    public GameObject testPattern;
    public int hp;
    public PoolKey key;


    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp == 10)
            {
                Pattern();
            }
        }
    }

    public void Start()
    {
        hp = 110;
        PoolManager.instance.BuildPool(key,transform);
        
    }

    public void Update()
    {
    }
    IEnumerator BossPatternCo()
    {
        yield return new WaitForSeconds(3f);
        GameObject obj = PoolManager.instance.UsePool(key);
        PoolManager.instance.ActivePool(obj);
        yield return new WaitForSeconds(5f);
        PoolManager.instance.ReturnPool(obj);
    }

    

    public void Pattern()
    {
        transform.position = new Vector3(0f,2f, 0f);
        StartCoroutine(BossPatternCo());

    }

    public void Hit(int atk)
    {
        Hp -= atk;
    }
}
