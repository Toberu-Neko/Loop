using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class PauseInvDescription : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent itemNameText;
    [SerializeField] private LocalizeStringEvent itemDescriptionText;

    [SerializeField] private LocalizedString defaultItemName;
    [SerializeField] private LocalizedString defaultItemDescription;

    public void Activate(SO_ItemsBase item)
    {
        gameObject.SetActive(true);

        itemNameText.StringReference = item.displayNameLocalization;
        itemDescriptionText.StringReference = item.descriptionLocalization;
    }

    public void Deactivate()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        gameObject.SetActive(false);
        ClearDescription();
    }

    public void ClearDescription()
    {
        itemNameText.StringReference = defaultItemName;
        itemDescriptionText.StringReference = defaultItemDescription;
    }


}
