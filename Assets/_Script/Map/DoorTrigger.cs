using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public event Action OnDoorTriggered;

    private PlayerInputHandler playerInputHandler;
    private bool inRange;
    [SerializeField] private SpriteRenderer SR;
    [SerializeField] private GameObject pressEObj;

    private void Awake()
    {
        SR.color = Color.red;
        pressEObj.SetActive(false);
    }

    private void Update()
    {
        if (inRange)
        {
            if (playerInputHandler.InteractInput)
            {
                OnDoorTriggered?.Invoke();
                SR.color = Color.green;
                Invoke(nameof(ChangeSRColor), 1f);
            }
        }
    }

    private void ChangeSRColor()
    {
        SR.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerInputHandler == null)
                playerInputHandler = collision.GetComponent<PlayerInputHandler>();

            inRange = true;
            pressEObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            pressEObj.SetActive(false);
        }
    }
}
