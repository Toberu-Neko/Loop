using System.Collections;
using System.Collections.Generic;
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
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.ReloadBaseScene();
    }
}
