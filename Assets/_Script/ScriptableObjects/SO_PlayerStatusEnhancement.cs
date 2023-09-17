using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enhancement", menuName = "Items/Enhancement")]
public class SO_PlayerStatusEnhancement : ScriptableObject
{
    [Header("Necessary Data")]
    public ItemDetails itemDetails;
    public Sprite itemSprite;

    [TextArea(5, 10)] public string itemDescription;

    [Header("Effects")]
    public float addMaxHealth = 10f;
    public float addDamage = 1f;


}
