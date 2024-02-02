using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class SO_ItemsBase : ScriptableObject
{
    [Header("Base")]
    public string ID;
    public string itemName = "Defult item name";
    public LocalizedString displayNameLocalization;
    public string displayName = "Defult display name";
    public LocalizedString shortDescriptionLocalization;
    public string shortDescription = "Defult short description";

    public LocalizedString descriptionLocalization;
    [TextArea(3, 10)]
    public string itemDescription = "Defult Description";

    public Sprite itemSprite;
}
