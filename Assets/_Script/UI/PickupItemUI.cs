using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class PickupItemUI : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent itemNameText;
    [SerializeField] private LocalizeStringEvent itemDescriptionText;

    [SerializeField] private LocalizedString defaultItemName;
    [SerializeField] private LocalizedString defaultItemDescription;

    [SerializeField] private GameObject firstSelectedObj;

    public void Active(LocalizedString name, LocalizedString description)
    {
        GameManager.Instance.PauseGame();
        gameObject.SetActive(true);
        UI_Manager.Instance.SetFirstSelectedObj(firstSelectedObj);

        if(name.IsEmpty || description.IsEmpty)
        {
            itemNameText.StringReference = defaultItemName;
            itemDescriptionText.StringReference = defaultItemDescription;
            return;
        }

        itemNameText.StringReference = name;
        itemDescriptionText.StringReference = description;
    }

    public void Deactive()
    {
        GameManager.Instance.ResumeGame();
        UI_Manager.Instance.FirstSelectedObjNull();
        gameObject.SetActive(false);
    }
}
