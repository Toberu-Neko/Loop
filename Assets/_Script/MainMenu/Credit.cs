using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Credit : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    
    public void OnBackClicked()
    {
        Deactivate();

        mainMenu.ActiveMenu(true);
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
