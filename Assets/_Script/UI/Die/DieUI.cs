using UnityEngine;

public class DieUI : MonoBehaviour
{
    [SerializeField] private GameObject firstTimePlayingObj;
    [SerializeField] private GameObject normalDieObj;

    public void Activate()
    {
        GameManager.Instance.PauseGame();
        gameObject.SetActive(true);

        if (DataPersistenceManager.Instance.GameData.firstTimePlaying)
        {
            firstTimePlayingObj.SetActive(true);
            normalDieObj.SetActive(false);
        }

        else
        {
            normalDieObj.SetActive(true);
            firstTimePlayingObj.SetActive(false);
        }
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
