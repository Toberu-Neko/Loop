using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Items/WeaponItem")]
public class SO_WeaponItem : SO_ItemsBase
{
    [Header("Weapon")]
    public bool unlockSword;
    public bool unlockGun;
    public bool unlockFist;
}
