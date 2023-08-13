using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot System/New LootDetails")]
public class LootDetails : ScriptableObject
{
    public string lootName;
    public Sprite lootSprite;
}
