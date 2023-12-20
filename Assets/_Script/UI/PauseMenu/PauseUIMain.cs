using UnityEngine;

public class PauseUIMain : MonoBehaviour
{
    [SerializeField] private GameObject pauseUIObj;
    [SerializeField] private TutorialMenu tutorialMenu;
    [SerializeField] private PauseInventoryMain pauseInventoryMain;
    [SerializeField] private OptionUI optionUI;
    [SerializeField] private PauseTeleport teleportUI;

    private void Awake()
    {
        tutorialMenu.gameObject.SetActive(false);
        pauseInventoryMain.gameObject.SetActive(false);
        teleportUI.gameObject.SetActive(false);
        optionUI.OnDeactivate += ActivateMenu;
    }

    private void OnDestroy()
    {
        optionUI.OnDeactivate -= ActivateMenu;
    }

    public void OnClickResumeButton()
    {
        DeactiveAllMenu();
    }

    public void OnClickGoToPreviousSavepoint()
    {
        DeactiveAllMenu();
        DataPersistenceManager.Instance.ReloadBaseScene();
    }

    public void OnClickGoToMainMenu()
    {
        DeactiveAllMenu();
        DataPersistenceManager.Instance.LoadMainMenuScene();
    }

    public void OnClickOption()
    {
        DeactivateMenu();
        optionUI.Activate();
    }

    public void OnClickTutorialButton()
    {
        DeactivateMenu();
        tutorialMenu.Activate();
    }

    public void OnClickInventoryButton()
    {
        DeactivateMenu();
        pauseInventoryMain.Activate();
    }

    public void OnClickTeleportButton()
    {
        DeactivateMenu();
        teleportUI.Activate();
    }

    public void ActivateMenu(bool init = false)
    {
        if(init)
        {
            pauseUIObj.SetActive(true);
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
        GameManager.Instance.ResumeGame();
        pauseUIObj.SetActive(false);
        tutorialMenu.Deactivate();
        pauseInventoryMain.Deactivate();
        optionUI.Deactivate();
        teleportUI.Deactivate();
        DeactivateMenu();
    }

}
