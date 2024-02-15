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

    [Header("Option")]
    [SerializeField] private OptionUI optionUI;

    [Header("Credit")]
    [SerializeField] private Credit credit;

    private void Awake()
    {
        saveSlotMenu.gameObject.SetActive(false);
        optionUI.gameObject.SetActive(false);
        credit.gameObject.SetActive(false);
        optionUI.OnDeactivate += ActiveMenu;
    }

    private void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadButton.interactable = false;
        }
    }


    protected void OnDestroy()
    {
        optionUI.OnDeactivate -= ActiveMenu;
    }

    public void OnNewGameClicked()
    {
        saveSlotMenu.ActiveMenu(false);

        DeactiveMenu();
    }

    public void OnContinueGameClicked()
    {
        LoadSceneManager.Instance.LoadScene(gameBaseScene.Name);
    }

    public void OnLoadButtobClicked()
    {
        saveSlotMenu.ActiveMenu(true);

        DeactiveMenu();
    }

    public void OnClickOptionButton()
    {
        optionUI.Activate();

        DeactiveMenu();
    }

    public void OnClickCreditButton()
    {
        credit.Activate();

        DeactiveMenu();
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    private void DisableAllButton()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActiveMenu(bool init = false)
    {
        gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
    }
}
