using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null;
    Dictionary<PoolKey, Queue<GameObject>> pool;
    /*
    [SerializeField]
    Transform poolRoot;
    */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        pool = new Dictionary<PoolKey, Queue<GameObject>>();
    }

    public void BuildPool(PoolKey key,Transform transform)
    {
        if (key == null)
        {
            Debug.Log("key 없음");
            return;
        }
        if (key.prewarmCount <= 0)
        {
            Debug.Log("key.prewarmCount 없음");
            return;
        }
        if (key.prefab == null)
        {
            Debug.Log("keyPrefab 없음");
            return;
        }
        //if(key.prewarmCount)

        Queue<GameObject> q = new Queue<GameObject>(key.prewarmCount);

        for (int i = 0; i < key.prewarmCount; i++)
        {
            GameObject obj = Instantiate(key.prefab, transform);
            obj.SetActive(false);

            PooledObject poolObj = obj.GetComponent<PooledObject>();
            if (poolObj == null)
            {
                poolObj = obj.AddComponent<PooledObject>();
            }

            poolObj.key = key;

            IPoolable poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.OnDeSpawned();

            }
            q.Enqueue(obj);
        }

        pool.Add(key, q);

    }

    public GameObject UsePool(PoolKey Key)
    {
        Queue<GameObject> q;
        if (!pool.TryGetValue(Key, out q))
        {
            return null;
        }
        if (q.Count <= 0)
        {
            return null;
        }
        GameObject obj = q.Dequeue();

        return obj;
    }

    public void ActivePool(GameObject obj)
    {
        if (obj == null) return;
        IPoolable able = obj.GetComponent<IPoolable>();
        if (able != null)
        {
            able.OnSpawned();
        }

        obj.SetActive(true);
    }

    public void ReturnPool(GameObject obj)
    {
        PooledObject poolobj = obj.GetComponent<PooledObject>();
        Queue<GameObject> q;
        if (!pool.TryGetValue(poolobj.key, out q))
        {
            return;
        }

        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null)
        {
          poolable.OnDeSpawned();

        }


       obj.SetActive(false);

      // obj.transform.SetParent(transform);
        q.Enqueue(obj);
    }

}
