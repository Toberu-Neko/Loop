using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Credit : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;

    [SerializeField] private GameObject firstSelectedObj;
    
    public void OnBackClicked()
    {
        Deactivate();

        mainMenu.ActiveMenu(true);
    }

    public void Activate()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedObj);

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        EventSystem.current.SetSelectedGameObject(null);

        gameObject.SetActive(false);
    }
}
