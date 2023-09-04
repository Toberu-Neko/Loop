using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIChangeSkill : MonoBehaviour
{
    [SerializeField] private PauseUIMain pauseUIMain;

    public void OnClickBackButton()
    {
        pauseUIMain.ActivateMenu();
        Deactivate();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
