using GameAssets.Scripts.Data;
using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Weapons;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Scripts.Players
{
    public class Player : MonoBehaviour
    {
     
        Animator animator;
        public PlayerStatus player;
        public Transform weaponHand;
        Weapon weapon;
       
       // public Image playerHp;
      //  public TextMeshPro text;
       

       
        public Animator Animator => animator;
      
        public void Attack()
        {
            weapon = GetComponentInChildren<Weapon>();
            if (weapon == null)
            {
                return;
            }
            weapon.HateAttack();
            weapon.hitCollider.isTrigger = true;
            StartCoroutine(TriggerCo());
            
        }
        IEnumerator TriggerCo()
        {
            yield return new WaitForSeconds(0.2f);
            weapon.hitCollider.isTrigger = false;
        }
        public void Awake()
        {
            animator= GetComponent<Animator>();
        }

        public void Update()
        {
          // GameManager.instance.Skill.Skill();
            
        }
        public void Start()
        {
          
        }

      
      
     

    }
}

