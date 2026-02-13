using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossPattern : MonoBehaviour//,IHitAble
{
    public GameObject testPattern;
    // public int hp;
    public PoolKey key;
    [SerializeField]
    Monster monster;


    /*
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
    */
    public void Awake()
    {
        if (monster == null)
        {
            monster=GetComponent<Monster>();
        }
    }
    public void Start()
    {
        //  hp = 110;
        PoolManager.instance.BuildPool(key,transform);
        StartCoroutine(WaitCo());
    }

    IEnumerator WaitCo()
    {
        yield return null;
        monster.bossPattern += Pattern;
    }
    public void OnEnable()
    {
        if(monster == null) { Debug.Log("안 붙어있어 짜샤 "); monster = GetComponent<Monster>(); }
        
    }
    public void OnDisable()
    {
        if (monster == null) { Debug.Log("안 붙어있어 짜샤 "); }
        monster.bossPattern -= Pattern;
    }
    public void Update()
    {
        
    }
    IEnumerator BossPatternCo()
    {
        monster.animator.SetTrigger("Pattern");
        yield return new WaitForSeconds(3f);
        GameObject obj = PoolManager.instance.UsePool(key);
        PoolManager.instance.ActivePool(obj);
        yield return new WaitForSeconds(5f);
        PoolManager.instance.ReturnPool(obj);
    }

    

    public void Pattern()
    {
        transform.position = new Vector3(70f,0.1f, 70f);
        StartCoroutine(BossPatternCo());

    }
    /*
    public void Hit(int atk)
    {
        Hp -= atk;
    }
    */
}
