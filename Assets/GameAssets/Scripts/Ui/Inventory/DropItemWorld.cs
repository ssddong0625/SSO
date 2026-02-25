using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemWorld : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;
    [SerializeField] LayerMask layerMask;

    public void Awake()
    {
        StartCoroutine(DestroyObjectCO());
    }

    IEnumerator DestroyObjectCO()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(((1<<other.gameObject.layer)& layerMask.value) == 0)
        {
            return;
        }
        // if (!other.CompareTag("Player")) return;
        InventorySystem inv = other.GetComponent<InventorySystem>();
        if (inv == null)
        {
            return;
        }

            if (inv.AddItem(itemData, amount))
            Destroy(gameObject);
    }
}
