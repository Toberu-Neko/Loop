using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipGrid : MonoBehaviour
{
    [SerializeField] private EquipmentSlot[] equipmentSlots;

    public void SetCanInterect(bool value)
    {
        foreach (var slot in equipmentSlots)
        {
            slot.SetCanInterect(value);
        }
    }
}
