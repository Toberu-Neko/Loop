using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class SO_ItemsBase : ScriptableObject
{
    [Header("Base")]
    public string ID;
    public LocalizedString displayNameLocalization;
    public LocalizedString shortDescriptionLocalization;
    public LocalizedString descriptionLocalization;

    public Sprite itemSprite;
}
