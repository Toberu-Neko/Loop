using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    [SerializeField] private PauseUIMain pauseUIMain;

    [SerializeField] private GameObject firstSelectedObj;
    public void Activate()
    {
        gameObject.SetActive(true);

        UI_Manager.Instance.SetFirstSelectedObj(firstSelectedObj);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        UI_Manager.Instance.FirstSelectedObjNull();
        pauseUIMain.ActivateMenu();
    }


}
