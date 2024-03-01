using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
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

    [SerializeField] private GameObject firstSelectedObj;

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
        ActiveMenu();
    }


    protected void OnDestroy()
    {
        optionUI.OnDeactivate -= ActiveMenu;
    }

    public void OnNewGameClicked()
    {
        DeactiveMenu();

        saveSlotMenu.ActiveMenu(false);
    }

    public void OnContinueGameClicked()
    {
        LoadSceneManager.Instance.LoadScene(gameBaseScene.Name);
    }

    public void OnLoadButtobClicked()
    {
        DeactiveMenu();

        saveSlotMenu.ActiveMenu(true);
    }

    public void OnClickOptionButton()
    {
        DeactiveMenu();

        optionUI.Activate();
    }

    public void OnClickCreditButton()
    {
        DeactiveMenu();

        credit.Activate();
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
        EventSystem.current.SetSelectedGameObject(firstSelectedObj);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
