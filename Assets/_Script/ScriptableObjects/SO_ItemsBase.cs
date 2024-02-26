using UnityEngine;
using UnityEngine.Localization;

public class SO_ItemsBase : ScriptableObject
{
    [Header("Base")]
    public string ID;
    public LocalizedString displayNameLocalization;
    public LocalizedString shortDescriptionLocalization;
    public LocalizedString descriptionLocalization;
    public LocalizedString popupTutorialLocalization;
    public int price = 1;

    public Sprite itemSprite;
}
