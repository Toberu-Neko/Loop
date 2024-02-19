using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemPrefab : DropableItemBase
{
    [HideInInspector] public SO_Chip lootSO;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventoryManager.Instance.AddChip(lootSO.ID);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

}


