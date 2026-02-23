using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Weapons;
using UnityEditor.ShaderGraph;
using System;
using Unity.VisualScripting;

public class PlayerEquip : MonoBehaviour
{
    [SerializeField]
    Transform weaponSocket;
    [SerializeField]
    Transform bowSocket;
    WeaponData currentWeapon;
    GameObject currentWeaponObj;

    [SerializeField]
    WeaponData startWeapon;
    
    [SerializeField]
    Animator playerAnimator;
    public Animator PlayerAnimator => playerAnimator;

    public WeaponData CurrentWeapon => currentWeapon;
    public WeaponData StartWeapon => startWeapon;
    public event Action<WeaponData> OnWeaponChanged;
    public GameObject CurrentWeaponObj => currentWeaponObj;
    public void Equip(WeaponData weapon)
    {
        currentWeapon = weapon;
        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
            currentWeaponObj = null;
        }
        if (weapon == null || weapon.prefab == null)
        {
            OnWeaponChanged?.Invoke(currentWeapon);
            return;
        }
        currentWeaponObj = Instantiate(weapon.prefab);
        OnWeaponChanged?.Invoke(weapon);

        Weapon weaponGrip = currentWeaponObj.GetComponent<Weapon>();
        if(weaponGrip == null || weaponGrip.GripPoint==null)
        {
            Debug.Log("weaponGrip ³Î");
            Destroy(currentWeaponObj);
            currentWeaponObj = startWeapon.prefab;
            OnWeaponChanged?.Invoke(currentWeapon);

            return;
        }


        /*
        if(weapon!=null && weapon.controller != null)
        {
            playerAnimator.runtimeAnimatorController = weapon.controller;
        }
        else
        {
            playerAnimator.runtimeAnimatorController = noneWeapon.controller;
        }
        */
        if (weapon.type == WeaponType.Bow)
        {
            currentWeaponObj.transform.SetParent(bowSocket);
            GripToSocket(currentWeaponObj.transform, weaponGrip.GripPoint, bowSocket);
            currentWeaponObj.transform.localScale = Vector3.one;
            OnWeaponChanged?.Invoke(currentWeapon);
        }
        else
        {
            currentWeaponObj.transform.SetParent(weaponSocket);
          GripToSocket(currentWeaponObj.transform,weaponGrip.GripPoint,weaponSocket);
          currentWeaponObj.transform.localScale = Vector3.one;
          OnWeaponChanged?.Invoke(currentWeapon);

        }
    }
    public void Unequip()
    {
        /*
        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
            currentWeaponObj = null;
        }
        */
        Equip(startWeapon);
    }

    public void GripToSocket(Transform weaponRoot, Transform grip, Transform socket)
    {
        weaponRoot.rotation = socket.rotation * Quaternion.Inverse(grip.localRotation);
        weaponRoot.position = socket.position - (weaponRoot.rotation * grip.localPosition);
    }

    public void AttackEvent(WeaponType type)
    {
        /*
        switch (type)
        {
            case WeaponType.Bow:
                var bow= currentWeaponObj.GetComponent<Bow>();
                bow.Attack();
                break;
            case WeaponType.Sword:
                var Sword =currentWeaponObj.GetComponent<Sword>();
                Sword.Attack(); 
                break;
            
        }
        */
        

        if (currentWeapon.type==WeaponType.Bow)
        {
            var bow = currentWeaponObj.GetComponent<Bow>();
            bow.Attack();
        }
        else if (currentWeapon.type == WeaponType.Sword)
        {
            var sword =currentWeaponObj.GetComponent<Sword>();
            sword.Attack();
        }
        else
        {
             
        }
    }

    /*
    public void EquipWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Equip(noneWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Equip(swordWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip(bowWepon);
        }
    }
    */


    public void Awake()
    {
        
    }
    public void Start()
    {
        OnWeaponChanged?.Invoke(startWeapon);
    }
    public void Update()
    {
     //   EquipWeapon();
    }

}

