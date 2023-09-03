using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot System/New LootDetails")]
public class LootSO : ScriptableObject
{
    public LootDetails lootDetails;
    public Sprite lootSprite;
}

[System.Serializable]
public class LootDetails
{
    public string lootName;

    public bool canEquipOnSword;
    public bool canEquipOnGun;
    public bool canEquipOnFist;
}

