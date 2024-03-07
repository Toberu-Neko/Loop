using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private GameObject savedNotificationObj;
    [SerializeField] private CanvasGroup inGameCanvas;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingObj;

    [Header("Change Scene UI")]
    [SerializeField] private GameObject changeSceneUI;
    [SerializeField] private Animator changeSceneAnimator;

    [Header("Pickup Item UI")]
    [SerializeField] private GameObject pickUpItemUIObj;
    private PickupItemUI pickUpItemUI;

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseUIObj;
    [SerializeField] private GameObject pauseUIMainObj;
    [SerializeField] private GameObject pauseUIChooseSkillObj;
    [SerializeField] private GameObject pauseUIMapObj;
    private PauseUIMain pauseUIMain;

    [Header("Savepoint UI")]
    [SerializeField] private GameObject savepointUIObj;
    [SerializeField] private GameObject savepointUIMainObj;
    [SerializeField] private GameObject savepointUIInventoryObj;
    [SerializeField] private GameObject savepointUITeleportObj;

    [Header("Shop UI")]
    [SerializeField] private GameObject shopUIObj;
    [SerializeField] private ShopUI shopUI;

    [Header("HUD")]
    [SerializeField] private PickupHUD pickupHUD;
    [SerializeField] private PickupHUD tutorialHUD;
    [SerializeField] private GameObject bossFightUIObj;
    private BossFightUI bossFightUI;

    [Header("Die UI")]
    [SerializeField] private DieUI dieUI;

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

        changeSceneUI.SetActive(false);
        savedNotificationObj.SetActive(false);

        pauseUIObj.SetActive(false);
        pauseUIMainObj.SetActive(false);
        pauseUIChooseSkillObj.SetActive(false);
        pauseUIMapObj.SetActive(false);
        pauseUIMain = pauseUIMainObj.GetComponent<PauseUIMain>();

        savepointUIObj.SetActive(false);
        savepointUIMainObj.SetActive(false);
        savepointUIInventoryObj.SetActive(false);
        savepointUITeleportObj.SetActive(false);

        savepointUIMain = savepointUIMainObj.GetComponent<SavepointUIMain>();
        savepointUIInventory = savepointUIInventoryObj.GetComponent<SavepointUIInventory>();

        bossFightUIObj.SetActive(false);
        bossFightUI = bossFightUIObj.GetComponent<BossFightUI>();

        pickUpItemUIObj.SetActive(false);
        pickUpItemUI = pickUpItemUIObj.GetComponent<PickupItemUI>();

        shopUIObj.SetActive(false);

        dieUI.gameObject.SetActive(false);

        loadingObj.SetActive(true);

        savePointNames = new();
        savepoints = new();
    }

    private void Start()
    {
        GameManager.Instance.OnChangeSceneGoUp += HandleChangeSceneGoUp;
        GameManager.Instance.OnChangeSceneGoDown += HandleChangeSceneGoDown;
        GameManager.Instance.OnChangeSceneGoLeft += HandleChangeSceneGoLeft;
        GameManager.Instance.OnChangeSceneGoRight += HandleChangeSceneGoRight;
        GameManager.Instance.OnChangeSceneFinished += HandleChangeSceneFinish;
        GameManager.Instance.OnSavepointInteracted += HandleSavePointInteraction;

        DataPersistenceManager.Instance.OnSave += HandleSave;

        LoadSceneManager.Instance.LoadingObj = loadingObj;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnChangeSceneGoUp -= HandleChangeSceneGoUp;
        GameManager.Instance.OnChangeSceneGoDown -= HandleChangeSceneGoDown;
        GameManager.Instance.OnChangeSceneGoLeft -= HandleChangeSceneGoLeft;
        GameManager.Instance.OnChangeSceneGoRight -= HandleChangeSceneGoRight;
        GameManager.Instance.OnChangeSceneFinished -= HandleChangeSceneFinish;
        GameManager.Instance.OnSavepointInteracted -= HandleSavePointInteraction;

        DataPersistenceManager.Instance.OnSave -= HandleSave;
    }

    private void Update()
    {
        if (inputHandler.ESCInput)
        {
            inputHandler.UseESCInput();

            if (!pauseUIObj.activeInHierarchy &&
                !savepointUIObj.activeInHierarchy &&
                !pickUpItemUIObj.activeInHierarchy &&
                !shopUIObj.activeInHierarchy)
            {
                OpenPauseMainUI();
            }
            else if (pauseUIObj.activeInHierarchy)
            {
                ClosePauseMainUI();
            }
            else if (savepointUIObj.activeInHierarchy)
            {
                CloseAllSavePointUI();
                savepointUIObj.SetActive(false);
            }
            else if (pickUpItemUIObj.activeInHierarchy)
            {
                pickUpItemUI.Deactive();
            }
            else if (shopUIObj.activeInHierarchy)
            {
                shopUI.Deactivate();
            }
        }

        if (inputHandler.TurnOffUI)
        {
            inputHandler.UseTurnOffUIInput();
            if (inGameCanvas.alpha == 0f)
            {
                inGameCanvas.alpha = 1f;
            }
            else
            {
                inGameCanvas.alpha = 0f;
            }
        }
    }

    public void ActiveDieUI()
    {
        dieUI.Activate();
    }

    public void ActiveBossUI(BossBase bossBase)
    {
        bossFightUI.Active(bossBase);
    }

    public void ActivePickupItemUI(LocalizedString name, LocalizedString description)
    {
        pickUpItemUI.Active(name, description);
    }

    public void ActivatePickupItemUIHUD(LocalizedString name)
    {
        pickupHUD.AddToQueue(name);
    }

    public void ActivateTutorialPopUpUI(LocalizedString description)
    {
        tutorialHUD.AddToQueue(description);
    }

    private void OpenPauseMainUI()
    {
        pauseUIMain.ActivateMenu(true);
        inputHandler.NResetAllInput();
    }

    private void ClosePauseMainUI()
    {
        pauseUIMain.DeactiveAllMenu();
    }


    public void CloseAllSavePointUI()
    {
        savepointUIMain.DeactiveAllMenu();
    }

    public void HandleSavePointInteraction(string savePointID, LocalizedString savePointName)
    {
        savepointUIMain.ActivateMenu(true);
        savepointUIMain.SetSavepointNameText(savePointName);

        inputHandler.NResetAllInput();
    }

    public void ResetAllInput()
    {
        inputHandler.NResetAllInput();
    }

    public void ActivateShopUI(string shopID, LocalizedString shopName)
    {
        shopUI.Activate(shopID, shopName);
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

    public void SetFirstSelectedObj(GameObject parent)
    {
        EventSystem.current.SetSelectedGameObject(parent);
    }

    public void FirstSelectedObjNull()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    #region Change Scene

    private void HandleChangeSceneGoUp()
    {
        changeSceneUI.SetActive(true);
        if (bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();
        changeSceneAnimator.SetTrigger("toUp");
    }

    private void HandleChangeSceneGoDown()
    {
        changeSceneUI.SetActive(true);
        if (bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();
        changeSceneAnimator.SetTrigger("toDown");
    }

    private void HandleChangeSceneGoLeft()
    {
        changeSceneUI.SetActive(true);
        if (bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();
        bossFightUIObj.SetActive(false);
        changeSceneAnimator.SetTrigger("toLeft");
    }
    public void HandleChangeSceneGoRight()
    {
        changeSceneUI.SetActive(true);
        if(bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();
        changeSceneAnimator.SetTrigger("toRight");
    }
    private void HandleChangeSceneFinish()
    {
        loadingObj.SetActive(false);

        if(changeSceneUI.activeInHierarchy)
            changeSceneAnimator.SetBool("finishLoading", true);
    }

    public void BlockPlayerSight()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("block");
    }

    #endregion
}
