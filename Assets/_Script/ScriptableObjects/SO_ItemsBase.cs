using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_ItemsBase : ScriptableObject
{
    [Header("Base")]
    public string itemName = "Defult item name";
    public string displayName = "Defult display name";
    public string shortDescription = "Defult short description";
    [TextArea(3, 10)]
    public string itemDescription = "Defult Description";
    public Sprite itemSprite;
}
