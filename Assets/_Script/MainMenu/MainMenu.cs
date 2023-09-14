using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MenuFirstSelecter
{
    [Header("Navigation")]
    [SerializeField] private SaveSlotMenu saveSlotMenu;

    [Header("Config")]
    [SerializeField] private SceneReference gameBaseScene;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadButton;

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotMenu.ActiveMenu(false);

        DeactiveMenu();
    }

    public void OnContinueGameClicked()
    {
        SceneManager.LoadScene(gameBaseScene.Name);
    }

    public void OnLoadButtobClicked()
    {
        saveSlotMenu.ActiveMenu(true);

        DeactiveMenu();
    }

    private void DisableAllButton()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActiveMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
    }

}
