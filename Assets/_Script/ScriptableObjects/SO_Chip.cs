using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot System/New LootDetails")]
public class SO_Chip : ScriptableObject
{
    [Header("Necessary Data")]
    public ItemDetails itemDetails;
    public Sprite itemSprite;
    [TextArea(5, 10)] public string itemDescription;

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

// for saving system
[System.Serializable]
public class ItemDetails
{
    public string lootName;

    public ItemDetails(string lootName)
    {
        this.lootName = lootName;
    }
}

