using System;
using UnityEngine;

public class PressEPickItemBase : DropableItemBase
{
    [SerializeField] private GameObject pickUpText;

    private PlayerInputHandler inputHandler;
    private bool inRange;
    protected event Action OnItemPicked;

    protected override void Awake()
    {
        base.Awake();

        pickUpText.SetActive(false);
        inRange = false;
    }

    protected override void Update()
    {
        base.Update();

        if (inRange)
        {
            if (inputHandler.InteractInput)
            {
                inputHandler.UseInteractInput();
                pickUpText.SetActive(false);

                OnItemPicked?.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (inputHandler == null)
                inputHandler = collision.GetComponent<PlayerInputHandler>();

            pickUpText.SetActive(true);
            inRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickUpText.SetActive(false);
            inRange = false;
        }
    }
}
