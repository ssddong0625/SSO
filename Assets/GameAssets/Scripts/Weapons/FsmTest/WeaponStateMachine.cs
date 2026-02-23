using GameAssets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponStateMachine : MonoBehaviour
{
    [SerializeField]
    PlayerEquip playerEquip;
    WeaponState currentState;
    NoneWeaponState none;
    SwordWeaponState sword;
    BowWeaponState bow;
    public WeaponState CurrentState => currentState;

    private void Awake()
    {
        none = new NoneWeaponState(this, playerEquip);
        sword = new SwordWeaponState(this, playerEquip);
        bow=new BowWeaponState(this,playerEquip);
    }

    private void OnEnable()
    {
        if (playerEquip != null)
        {
            playerEquip.OnWeaponChanged -= WeaponChanged;
            playerEquip.OnWeaponChanged += WeaponChanged;
        }
    }
    private void OnDisable()
    {
        if(playerEquip != null)
        {
            playerEquip.OnWeaponChanged -= WeaponChanged;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playerEquip.OnWeaponChanged+=WeaponChanged;
    }
    private void Update()
    {
        currentState?.UpdateState();
    }
    public void WeaponChanged(WeaponData data)
    {
        WeaponState state = none;
        if (data != null)
        {
            if (data.type == WeaponType.Sword)
            {
                state = sword;
            }
            else if (data.type == WeaponType.Bow)
            {
                state = bow;
            }
            else
            {
                state = none;
            }
            Debug.Log(state);
        }
        ChangeState(state, data);
    }

    
    public void ChangeState(WeaponState state,WeaponData data)
    {
        if (currentState == state) { return; }
        currentState?.Exit();
        currentState = state;
        currentState?.Enter(data);
    }

  
}
