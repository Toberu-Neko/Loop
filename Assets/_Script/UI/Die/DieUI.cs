using UnityEngine;

public class DieUI : MonoBehaviour
{
    public void Activate()
    {
        GameManager.Instance.PauseGame();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        GameManager.Instance.ResumeGame();

        if (DataPersistenceManager.Instance.GameData.firstTimePlaying)
        {
            GameManager.Instance.LoadStartAnimScene();
        }

        else
        {
            gameObject.SetActive(false);
            DataPersistenceManager.Instance.ReloadBaseScene();
        }
    }
}
