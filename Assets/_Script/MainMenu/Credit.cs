using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MenuFirstSelecter
{
    [SerializeField] private MainMenu mainMenu;
    
    public void OnBackClicked()
    {
        mainMenu.ActiveMenu(true);
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
