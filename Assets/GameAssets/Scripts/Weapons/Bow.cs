using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using GameAssets.Scripts.Players;
using GameAssets.Scripts.Weapons;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bow : Weapon
{
    [SerializeField] private PoolKey key;        
    [SerializeField] private PoolKey changeKey;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] Player player;
    int fastAttack=1;
    int test = 1;
    private PoolKey currentKey;
    bool bowAttack;
    BowSkillTypeFour bowSkill = new BowSkillTypeFour();
    BowSkillTypeOne typeOne= new BowSkillTypeOne();
    BowSkillTypeTwo typeTwo= new BowSkillTypeTwo();
    BowSkillTypeThree typeThree= new BowSkillTypeThree();
    public BowSkillTypeFour BowSkill => bowSkill;
    public BowSkillTypeOne TypeOne => typeOne;
    public PoolKey Key => key;
    public PoolKey CurrentKey
    {
        get { return currentKey; }
        set {  currentKey = value; }
    }
    public PoolKey ChangeKey => changeKey;
    public LayerMask LayerMask => layerMask;
    public int FastAttack
    {
        get { return fastAttack; }
        set { fastAttack = value; }
    }
    public int Test
    {
        get { return test; }
        set {  test = value; }
    }
    float[] skillCoolTime= { 5f, 10f, 3f } ;
    float[] nextSkillTime =new float[3];

    public bool CanUseSkill(int index)
    {
        return Time.time >= nextSkillTime[index];
    }
    public void StartCoolDown(int index)
    {
        nextSkillTime[index] = Time.time + skillCoolTime[index];
    }

    public float GetRemainTIme(int index)
    {
        return Mathf.Max(0f, nextSkillTime[index] - Time.time);
    }
    public float GetSkillDuration(int index) => skillCoolTime[index];

    private void OnEnable()
    {
        currentKey = key;
         EnsurePool(currentKey);
        
    }

    public void SkillTypeOne()
    {
        if (!CanUseSkill(0)) { return; }
        StartCoroutine(typeOne.ChangeCo(this));
        StartCoolDown(0);
    }
    public void SkillTypeTwo()
    {
        if (!CanUseSkill(1)) { return; }
        StartCoroutine(typeTwo.AttackSpeedChangeCo(this));
        StartCoolDown(1);
    }
    public void SkillTypeThree()
    {
        if (!CanUseSkill(2)) { return; }
        typeThree.SkillThree(this);
        StartCoolDown(2);
    }

   
    public void EnsurePool(PoolKey key)
    {
        if (PoolManager.instance == null) return;

       
        PoolManager.instance.BuildPool(key, PoolManager.instance.transform);

    }
    

    public override void Attack()
    {
        Vector3 firePoint = GameManager.instance.Player.transform.position + GameManager.instance.Player.transform.forward * 0.6f + Vector3.up * 1.2f;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

        Vector3 hitPoint = ray.origin + ray.direction * 10f; 
        Monster mon = null;
        IHitAble hits = null;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            hitPoint = hit.point;
            mon = hit.collider.GetComponentInParent<Monster>();
            hits = hit.collider.GetComponent<IHitAble>();
        }

        if (hits != null)
        {
            mon?.SetAttacker(transform);
            hits.Hit(data.atk);
        }

        if (firePoint != null)
            SpawnArrowVfx(firePoint, hitPoint);
    }


    public void Attacking()
    {

        if (Input.GetMouseButton(1))
        {
            GameManager.instance.PlayerEquip.PlayerAnimator.SetTrigger("Ready");
            GameManager.instance.PlayerEquip.PlayerAnimator.ResetTrigger("GoHome");
            GameManager.instance.UiManager.crossHead.gameObject.SetActive(true);
        }
        else
        {
            GameManager.instance.PlayerEquip.PlayerAnimator.SetTrigger("GoHome");
            GameManager.instance.PlayerEquip.PlayerAnimator.ResetTrigger("Ready");
            GameManager.instance.UiManager.crossHead.gameObject.SetActive(false);
        }

        bool aiming = Input.GetMouseButton(1);
        if (GameManager.instance.PlayerEquip.BowAttack)
        {
            if (aiming && Input.GetMouseButtonDown(0))
            {
                if (Time.time >= nextAttack)
                {

                    nextAttack = Time.time + data.attackCoolDown/test;
                    GameManager.instance.PlayerEquip.PlayerAnimator.SetTrigger("Attack");
                    GameManager.instance.PlayerEquip.PlayerAnimator.SetFloat("AttackSpeed", fastAttack);
                }

            }
        }
       
    }

    IEnumerator SkillTwo()
    {
        yield return new WaitForSeconds(5f);
        fastAttack = 1;
        test = 1;
    }
    public void Skill2()
    {
        fastAttack = 10;
        test = 10;
        StartCoroutine(SkillTwo());
        
    }


    public void Skill()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

        Vector3 hitPoint = ray.origin + ray.direction * 50f;
        Monster mon = null;
        IHitAble hits = null;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            hitPoint = hit.point;
            mon = hit.collider.GetComponentInParent<Monster>();
            hits = hit.collider.GetComponent<IHitAble>();
        }

        if (hits != null)
        {
            mon?.SetAttacker(transform);
            hits.Hit(10 * data.atk);
        }
        Vector3 firePoint = GameManager.instance.Player.transform.position + GameManager.instance.Player.transform.forward * 0.6f + Vector3.up * 1.2f;
        if (firePoint != null)
            SpawnArrowVfx(firePoint, hitPoint);
    }

    public void SpawnArrowVfx(Vector3 start, Vector3 end)
    {
        EnsurePool(currentKey);
        if (PoolManager.instance == null) return;

        GameObject obj = PoolManager.instance.UsePool(currentKey);
        if (obj == null) return; 

        obj.transform.position = start;
        obj.transform.rotation = Quaternion.LookRotation((end - start).normalized);

        PoolManager.instance.ActivePool(obj);

        ArrowVfxPooled vfx = obj.GetComponent<ArrowVfxPooled>();
        if (vfx != null)
            vfx.Fire(start, end);
    }


}