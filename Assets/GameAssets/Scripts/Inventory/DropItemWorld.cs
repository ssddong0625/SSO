using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemWorld : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        InventorySystem inv = other.GetComponent<InventorySystem>();
        if (inv == null) return;

        if (inv.AddItem(itemData, amount))
            Destroy(gameObject);
    }
}
