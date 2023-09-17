using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Treasure", menuName = "Data/Treasure")]
public class SO_Treasure : ScriptableObject
{
    public enum TreasureType
    {
        Chip,
        Item,
        Weapon,
        Armor,
        Accessory,
        Skill,
        None
    }

}
