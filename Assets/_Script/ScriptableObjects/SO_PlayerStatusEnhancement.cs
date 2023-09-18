using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enhancement", menuName = "Items/Enhancement")]
public class SO_PlayerStatusEnhancement : SO_ItemsBase
{

    [Header("Effects")]
    public float addMaxHealth = 10f;
    public float addDamage = 1f;


}
