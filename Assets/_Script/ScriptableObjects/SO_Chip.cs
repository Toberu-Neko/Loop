using UnityEngine;

[CreateAssetMenu(fileName = "New Chip", menuName = "Items/New Chip")]
public class SO_Chip : SO_ItemsBase
{
    [Header("Available Equipment Type")]
    public bool canEquipOnSword;
    public bool canEquipOnGun;
    public bool canEquipOnFist;

    [Header("Effects(%)")]
    public MultiplierData multiplierData;
}

[System.Serializable]
public class MultiplierData
{
    public float damageMultiplier;
    public float attackSpeedMultiplier;
    public float chargeSpeedMultiplier;

    public MultiplierData()
    {
        damageMultiplier = 1f;
        attackSpeedMultiplier = 1f;
        chargeSpeedMultiplier = 1f;
    }
}


