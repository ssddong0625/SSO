using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour
{
    [SerializeField]
    PoolKey key;
    [SerializeField]
    Transform spone;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PoolManager.instance.BuildPool(key,spone);
        
    }


    public void ShotFire()
    {
        GameObject obj = PoolManager.instance.UsePool(key);
        if (obj==null)
        {
            return;
        }
        obj.transform.position = spone.position;
        obj.transform.rotation = spone.rotation;
        obj.transform.SetParent(null);
        PoolManager.instance.ActivePool(obj);
    }
    IEnumerator ShotCo()
    {
        ShotFire();
        yield return new WaitForSeconds(5f);

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
