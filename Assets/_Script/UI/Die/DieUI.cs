using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieUI : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedObj;
    public void Activate()
    {
        GameManager.Instance.PauseGame();
        UI_Manager.Instance.SetFirstSelectedObj(firstSelectedObj);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        GameManager.Instance.ResumeGame();
        UI_Manager.Instance.FirstSelectedObjNull();
        gameObject.SetActive(false);
        DataPersistenceManager.Instance.ReloadBaseScene();
    }
}
