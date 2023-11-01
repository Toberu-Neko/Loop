using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : PressEPickItemBase
{
    [HideInInspector] public SO_ItemsBase itemSO;
    [HideInInspector] public bool isRetunToPool = false;
    protected override void OnEnable()
    {
        base.OnEnable();

        OnItemPicked += HandlePickUp;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnItemPicked -= HandlePickUp;
    }

    private void HandlePickUp()
    {
        UI_Manager.Instance.ActivePickupItemUI(itemSO.displayName, itemSO.itemDescription);

        if(itemSO is SO_Chip)
        {
            PlayerInventoryManager.Instance.AddChip(itemSO.itemName);
        }
        else if(itemSO is SO_ConsumeableItem)
        {
            PlayerInventoryManager.Instance.AddConsumableItem(itemSO.itemName);
        }
        else if(itemSO is SO_PlayerStatusEnhancement)
        {
            PlayerInventoryManager.Instance.AddPlayerStatusEnhancementItem(itemSO.itemName);
        }
        else
        {
            Debug.LogError("This item should not be added by this script.");
        }



        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
