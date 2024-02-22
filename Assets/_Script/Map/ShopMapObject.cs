using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMapObject : MonoBehaviour
{
    [SerializeField] private GameObject keyboardTutorialObject;
    [SerializeField] private GameObject gamepadTutorialObject;

    [SerializeField] private SO_Shop data;
    private PlayerInputHandler inputHandler;
    private bool inRange;

    private void OnEnable()
    {
        keyboardTutorialObject.SetActive(false);
        gamepadTutorialObject.SetActive(false);
        inRange = false;
    }


    private void Update()
    {
        if (inRange)
        {
            if (inputHandler.InteractInput)
            {
                inputHandler.UseInteractInput();
                keyboardTutorialObject.SetActive(false);
                gamepadTutorialObject.SetActive(false);

                UI_Manager.Instance.ActivateShopUI(data.shopID, data.shopName);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (inputHandler == null)
                inputHandler = collision.GetComponent<PlayerInputHandler>();
            
            if (GameManager.Instance.PlayerInput.currentControlScheme == "Gamepad")
            {
                gamepadTutorialObject.SetActive(true);
            }
            else
            {
                keyboardTutorialObject.SetActive(true);
            }
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            keyboardTutorialObject.SetActive(false);
            gamepadTutorialObject.SetActive(false);
            inRange = false;
        }
    }

}
