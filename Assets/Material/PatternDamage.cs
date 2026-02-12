using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDamage : MonoBehaviour
{
    public int atk = 5555;

    [SerializeField]
    LayerMask layermask;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layermask.value) == 0) { return; }
        IHitAble hit = other.GetComponent<IHitAble>();
        hit.Hit(atk);
    }
 }
