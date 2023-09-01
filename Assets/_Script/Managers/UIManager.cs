using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }
    private GameManager gameManager;
    private DataPersistenceManager dataPersistenceManager;

    [SerializeField] private PlayerInputHandler inputHandler;

    [SerializeField] private GameObject changeSceneUI;
    [SerializeField] private Animator changeSceneAnimator;
    [SerializeField] private GameObject savedNotificationObj;

    [SerializeField] private GameObject pauseMainUI;

    [SerializeField] private GameObject savepointUIObj;
    [SerializeField] private GameObject savepointUIMainObj;
    [SerializeField] private GameObject savepointUIInventoryObj;
    private SavepointUIMain savepointUIMain;
    private SavepointUIInventory savepointUIInventory;

    private List<Savepoint> savepoints = new();
    private List<string> savePointNames = new();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pauseMainUI.SetActive(false);
        changeSceneUI.SetActive(false);
        savepointUIMainObj.SetActive(false);
        savedNotificationObj.SetActive(false);
        savepointUIInventoryObj.SetActive(false);
        savepointUIObj.SetActive(false);

        savepointUIMain = savepointUIMainObj.GetComponent<SavepointUIMain>();
        savepointUIInventory = savepointUIInventoryObj.GetComponent<SavepointUIInventory>();

        savePointNames = new();
        savepoints = new();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        dataPersistenceManager = DataPersistenceManager.Instance;

        gameManager.OnChangeSceneGoUp += HandleChangeSceneGoUp;
        gameManager.OnChangeSceneGoDown += HandleChangeSceneGoDown;
        gameManager.OnChangeSceneGoLeft += HandleChangeSceneGoLeft;
        gameManager.OnChangeSceneGoRight += HandleChangeSceneGoRight;
        gameManager.OnChangeSceneFinished += HandleChangeSceneFinish;

        dataPersistenceManager.OnSave += HandleSave;
    }

    private void OnDisable()
    {
        gameManager.OnChangeSceneGoUp -= HandleChangeSceneGoUp;
        gameManager.OnChangeSceneGoDown -= HandleChangeSceneGoDown;
        gameManager.OnChangeSceneGoLeft -= HandleChangeSceneGoLeft;
        gameManager.OnChangeSceneGoRight -= HandleChangeSceneGoRight;
        gameManager.OnChangeSceneFinished -= HandleChangeSceneFinish;

        dataPersistenceManager.OnSave -= HandleSave;

        foreach (var savepoint in savepoints)
        {
            savepoint.OnSavePointInteract -= HandleSavePointInteraction;
        }
    }

    private void Update()
    {
        if (inputHandler.ESCInput)
        {
            inputHandler.UseESCInput();

            if (!pauseMainUI.activeInHierarchy && 
                !savepointUIObj.activeInHierarchy)
            {
                OpenPauseMainUI();
            }
            else if (pauseMainUI.activeInHierarchy)
            {
                ClosePauseMainUI();
            }
            else if (savepointUIObj.activeInHierarchy)
            {
                CloseAllSavePointUI();
                savepointUIObj.SetActive(false);
            }
        }
    }

    public void RegisterSavePoints(Savepoint savePoint)
    {
        savepoints.Add(savePoint);
        if(savePointNames.Contains(savePoint.SavePointName))
        {
            Debug.LogError("Savepoint name already exists! Check: " + savePoint.SavePointName);
            return;
        }

        savePointNames.Add(savePoint.SavePointName);


        savePoint.OnSavePointInteract += HandleSavePointInteraction;
    }

    private void OpenPauseMainUI()
    {
        pauseMainUI.SetActive(true);
        gameManager.PauseGame();
    }

    private void ClosePauseMainUI()
    {
        pauseMainUI.SetActive(false);
        gameManager.ResumeGame();
    }


    public void CloseAllSavePointUI()
    {
        savepointUIMain.DeactiveAllMenu();
    }

    public void HandleSavePointInteraction(string savePointName, string sceneName)
    {
        savepointUIObj.SetActive(true);
        savepointUIMain.ActiveMenu();
        savepointUIMain.SetSavepointNameText(savePointName);

        gameManager.SavePointInteracted();
    }

    private void HandleSave()
    {
        savedNotificationObj.SetActive(true);
        StopCoroutine(nameof(DisableSavedObj));
        StartCoroutine(nameof(DisableSavedObj));
    }

    private IEnumerator DisableSavedObj()
    {
        yield return new WaitForSecondsRealtime(1f);
        savedNotificationObj.SetActive(false);
    }

    #region Change Scene

    private void HandleChangeSceneGoUp()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toUp");
    }

    private void HandleChangeSceneGoDown()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toDown");
    }

    private void HandleChangeSceneGoLeft()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toLeft");
    }
    private void HandleChangeSceneGoRight()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toRight");
    }
    private void HandleChangeSceneFinish()
    {
        if(changeSceneUI.activeInHierarchy)
            changeSceneAnimator.SetBool("finishLoading", true);
    }

    #endregion
}
