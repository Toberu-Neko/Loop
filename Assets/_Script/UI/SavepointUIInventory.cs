using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavepointUIInventory : MonoBehaviour
{
    [SerializeField] private SavepointUIMain savepointUIMain;

    public void OnClickBackButton()
    {
        DeactiveMenu();
        savepointUIMain.ActiveMenu();
    }

    public void ActiveMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactiveMenu()
    {
        gameObject.SetActive(false);
    }
}
