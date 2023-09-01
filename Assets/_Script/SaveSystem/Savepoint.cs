using System;
using UnityEngine;

public class Savepoint : MonoBehaviour, IDataPersistance
{
    [field: SerializeField] public string SavePointName { get; private set; }
    [field: SerializeField] public Transform TeleportTransform { get; private set; }

    [SerializeField] private GameObject pressEObject;
    public string SceneName { get; private set; }
    private PlayerInputHandler inputHandler;

    public event Action<string, string> OnSavePointInteract;

    private bool inRange;
    private bool isSavePointActive = false;

    private void Awake()
    {
        isSavePointActive = false;
    }

    private void OnEnable()
    {
        pressEObject.SetActive(false);
        inRange = false;
    }

    private void Start()
    {
        UI_Manager.Instance.RegisterSavePoints(this);
    }

    private void Update()
    {
        if (inRange)
        {
            if (inputHandler.InteractInput)
            {
                isSavePointActive = true;
                inputHandler.UseInteractInput();
                pressEObject.SetActive(false);

                // Go to UI manager
                OnSavePointInteract?.Invoke(SavePointName, SceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (inputHandler == null)
                inputHandler = collision.GetComponent<PlayerInputHandler>();

            pressEObject.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pressEObject.SetActive(false);
            inRange = false;
        }
    }

    public void SetSceneName(string name)
    {
        SceneName = name;
    }

    public void LoadData(GameData data)
    {
        if(SavePointName == null)
        {
            Debug.LogError("SavePointName is null");
            return;
        }
        data.activatedSavepoints.TryGetValue(SavePointName, out isSavePointActive);
    }

    public void SaveData(GameData data)
    {
        if(data.activatedSavepoints.ContainsKey(SavePointName))
        {
            data.activatedSavepoints.Remove(SavePointName);
        }
        data.activatedSavepoints.Add(SavePointName, isSavePointActive);
    }
}
