using System;
using UnityEngine;
using UnityEngine.Localization;

public class Savepoint : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject keyboardTutorialObject;
    [SerializeField] private GameObject gamepadTutorialObject;
    [field: SerializeField] public SO_Savepoint SavePointData { get; private set; }
    [field: SerializeField] public Transform TeleportTransform { get; private set; }

    private PlayerInputHandler inputHandler;

    public event Action<string, LocalizedString> OnSavePointInteract;

    private bool inRange;
    private bool isSavePointActive;
    private bool finishTutorial = false;


    private void OnEnable()
    {
        keyboardTutorialObject.SetActive(false);
        gamepadTutorialObject.SetActive(false);
        inRange = false;
        finishTutorial = false;
    }

    private void Start()
    {
        GameManager.Instance.RegisterSavePoints(this);
    }

    private void Update()
    {
        if (inRange)
        {
            if (inputHandler.InteractInput)
            {
                isSavePointActive = true;
                inputHandler.UseInteractInput();
                keyboardTutorialObject.SetActive(false);
                gamepadTutorialObject.SetActive(false);

                // Go to game manager
                OnSavePointInteract?.Invoke(SavePointData.savepointID, SavePointData.savepointName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (inputHandler == null)
                inputHandler = collision.GetComponent<PlayerInputHandler>();

            finishTutorial = true;
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


    public void LoadData(GameData data)
    {
        if(SavePointData == null)
        {
            Debug.LogError("SavePointData is null");
            return;
        }

        if(SavePointData.savepointID == null)
        {
            Debug.LogError("SavePointID is null");
            return;
        }
        data.savepoints.TryGetValue(SavePointData.savepointID, out SavepointDetails details);

        if (details != null)
        {
            isSavePointActive = details.isActivated;
        }
    }

    public void SaveData(GameData data)
    {
        if (finishTutorial)
        {
            data.finishTutorial = true;
        }

        if(data.savepoints.ContainsKey(SavePointData.savepointID))
        {
            data.savepoints.Remove(SavePointData.savepointID);
        }
        data.savepoints.Add(SavePointData.savepointID, new SavepointDetails(isSavePointActive, TeleportTransform.position));
    }
}
