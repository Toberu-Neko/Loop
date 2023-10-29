using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupChip : PressEPickItemBase
{
    [HideInInspector] public SO_Chip chipSO;
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
        UI_Manager.Instance.ActivePickupItemUI(chipSO.itemName, chipSO.itemDescription);
        PlayerInventoryManager.Instance.AddChip(chipSO.itemName);

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
