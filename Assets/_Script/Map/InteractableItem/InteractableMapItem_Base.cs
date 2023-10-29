using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableMapItem_Base : MonoBehaviour
{
    [SerializeField] private GameObject pressEText;
    protected bool interactable;


    private PlayerInputHandler inputHandler;
    private bool inRange;
    protected event Action OnInteract;

    protected virtual void Awake()
    {
        pressEText.SetActive(false);
        inRange = false;
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        if (inRange && interactable)
        {
            if (inputHandler.InteractInput)
            {
                inputHandler.UseInteractInput();
                pressEText.SetActive(false);

                OnInteract?.Invoke();
            }
        }
    }

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && interactable)
        {
            if (inputHandler == null)
                inputHandler = collision.GetComponent<PlayerInputHandler>();

            pressEText.SetActive(true);
            inRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressEText.SetActive(false);
            inRange = false;
        }
    }
}
