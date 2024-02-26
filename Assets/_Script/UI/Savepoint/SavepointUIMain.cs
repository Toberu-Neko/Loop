using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class SavepointUIMain : MonoBehaviour
{
    [SerializeField] private GameObject savepointUIObj;
    [SerializeField] private LocalizeStringEvent savepointNameText;
    [SerializeField] private LocalizedString defaultSavepointName;

    [SerializeField] private SavepointUIInventory savepointUIInventory;
    [SerializeField] private SavepointUIChangeSkill savepointUIChangeSkill;
    [SerializeField] private SavepointUITeleport savepointUITeleport;

    [SerializeField] private GameObject firstSelectedObj;


    public void SetSavepointNameText(LocalizedString name)
    {
        savepointNameText.StringReference = name;
    }

    public void OnClickSaveButton()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void OnClickTeleportButton()
    {
        DeactivateMenu();
        savepointUITeleport.Activate();
    }

    public void OnClickChangeSkillWeapon()
    {
        DeactivateMenu();
        savepointUIChangeSkill.Activate();
    }

    public void OnClickBackButton()
    {
        DeactiveAllMenu();
    }

    public void OnClickInventoryButton()
    {
        DeactivateMenu();
        savepointUIInventory.ActiveMenu();
    }

    public void OnClickGoToMainMenu()
    {
        DeactiveAllMenu();
        DataPersistenceManager.Instance.LoadMainMenuScene();
    }

    public void ActivateMenu(bool init = false)
    {
        if (init)
        {
            savepointUIObj.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        gameObject.SetActive(true);
        UI_Manager.Instance.SetFirstSelectedObj(firstSelectedObj);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
        UI_Manager.Instance.FirstSelectedObjNull();
    }

    public void DeactiveAllMenu()
    {
        savepointUIInventory.Deactivate();
        savepointUIChangeSkill.Deactivate();
        savepointUITeleport.Deactivate();
        savepointUIObj.SetActive(false);
        GameManager.Instance.ResumeGame();
        DeactivateMenu();
        DataPersistenceManager.Instance.ReloadBaseScene();
    }
}
