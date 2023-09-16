using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupChip : PressEPickItemBase
{
    [HideInInspector] public LootSO lootSO;

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
        UI_Manager.Instance.ActivePickupItemUI(lootSO.itemDetails.lootName, lootSO.itemDescription);
        PlayerInventoryManager.Instance.AddChip(lootSO.itemDetails);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
