using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private MainMenu mainMenu;
    
    [Header("Menu")]
    private SaveSlot[] saveSlots;

    [SerializeField] Button backButton;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!isLoadingGame)
        {
            DataPersistenceManager.Instance.NewGame();
        }

        DataPersistenceManager.Instance.ReloadBaseScene();
    }

    public void OnBackButtonClicked()
    {
        mainMenu.ActiveMenu();
        DeactiveMenu();
    }

    public void ActiveMenu(bool isLoadingGame)
    {
        gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out GameData profileData);
            saveSlot.SetData(profileData);

            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);

                if(firstSelected == backButton.gameObject)
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
            backButton.interactable = false;
    }
}
