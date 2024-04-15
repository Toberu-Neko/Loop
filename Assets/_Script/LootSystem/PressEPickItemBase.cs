using System;
using UnityEngine;

public class PressEPickItemBase : DropableItemBase
{
    [SerializeField] private GameObject keyboardTutorialObject;
    [SerializeField] private GameObject gamepadTutorialObject;
    [SerializeField] private Sound pickUpSFX;
    [field: SerializeField] public bool PressE { get; private set; } = false;

    private PlayerInputHandler inputHandler;
    private bool inRange;
    protected event Action OnItemPicked;

    protected override void Awake()
    {
        base.Awake();

        keyboardTutorialObject.SetActive(false);
        gamepadTutorialObject.SetActive(false);
        inRange = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnItemPicked += HandlePick;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnItemPicked -= HandlePick;
    }

    private void HandlePick()
    {
        AudioManager.Instance.PlaySoundFX(pickUpSFX, transform, AudioManager.SoundType.twoD);
    }

    protected override void Update()
    {
        base.Update();

        if (inRange)
        {
            if (inputHandler.InteractInput)
            {
                inputHandler.UseInteractInput();
                inputHandler.NResetAllInput();
                keyboardTutorialObject.SetActive(false);
                gamepadTutorialObject.SetActive(false);

                OnItemPicked?.Invoke();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PressE)
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
            else
            {
                OnItemPicked?.Invoke();
            }
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
