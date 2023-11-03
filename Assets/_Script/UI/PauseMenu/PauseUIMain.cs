using UnityEngine;

public class PauseUIMain : MonoBehaviour
{
    [SerializeField] private GameObject pauseUIObj;
    [SerializeField] private TutorialMenu tutorialMenu;

    private void Awake()
    {
        tutorialMenu.gameObject.SetActive(false);
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

    public void OnClickTutorialButton()
    {
        DeactivateMenu();
        tutorialMenu.Activate();
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
        DeactivateMenu();
    }

}
