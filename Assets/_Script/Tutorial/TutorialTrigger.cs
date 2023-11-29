using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField] private GameObject keyboardTutorialObject;
    [SerializeField] private GameObject gamepadTutorialObject;

    private void Awake()
    {
        keyboardTutorialObject.SetActive(false);
        gamepadTutorialObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.Instance.PlayerInput.currentControlScheme == "Gamepad")
            {
                gamepadTutorialObject.SetActive(true);
            }
            else
            {
                keyboardTutorialObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyboardTutorialObject.SetActive(false);
            gamepadTutorialObject.SetActive(false);
        }
    }
}
