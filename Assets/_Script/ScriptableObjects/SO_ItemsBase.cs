using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_ItemsBase : ScriptableObject
{
    public string itemName = "Defult item name";
    [TextArea(3, 10)]
    public string itemDescription = "Defult Description";
    public Sprite itemSprite;
}
