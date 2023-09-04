using UnityEngine;

public class PauseUIMain : MonoBehaviour
{
    [SerializeField] private GameObject pauseUIObj;
    [SerializeField] private PauseUIChangeSkill pauseUIChangeSkill;

    public void OnClickResumeButton()
    {
        DeactiveAllMenu();
    }

    public void OnClickChangeSkillButton()
    {
        pauseUIChangeSkill.Activate();
        DeactivateMenu();
    }

    public void OnClickGoToPreviousSavepoint()
    {
        DeactiveAllMenu();
        DataPersistenceManager.Instance.ReloadBaseScene();
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
        pauseUIChangeSkill.Deactivate();
        DeactivateMenu();
    }

}
