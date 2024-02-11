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
        savepointUIInventory.ActiveMenu();
        DeactivateMenu();
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
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    public void DeactiveAllMenu()
    {
        savepointUIInventory.Deactivate();
        savepointUIChangeSkill.Deactivate();
        savepointUITeleport.Deactivate();
        savepointUIObj.SetActive(false);
        GameManager.Instance.ResumeGame();
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.ReloadBaseScene();
    }
}
