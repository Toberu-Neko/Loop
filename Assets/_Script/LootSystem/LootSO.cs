using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot System/New LootDetails")]
public class LootSO : ScriptableObject
{
    [Header("Necessary Data")]
    public LootDetails lootDetails;
    public Sprite lootSprite;

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

    public MultiplierData()
    {
        damageMultiplier = 1f;
        attackSpeedMultiplier = 1f;
    }
}

[System.Serializable]
public class LootDetails
{
    // for saving system
    public string lootName;
}

