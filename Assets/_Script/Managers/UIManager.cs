using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.Localization;
using UnityEngine.Video;

/// <summary>
/// This manager is responsible for managing all UI in the ingame scene.
/// </summary>
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    #region UI Variables
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField] private GameObject savedNotificationObj;
    [SerializeField] private CanvasGroup inGameCanvas;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private HealthBar loadingBar;

    [Header("Change Scene UI")]
    [SerializeField] private GameObject changeSceneUI;
    [SerializeField] private Animator changeSceneAnimator;

    [Header("Pickup Item UI")]
    [SerializeField] private GameObject pickUpItemUIObj;
    private PickupItemUI pickUpItemUI;

    [SerializeField] private GameObject tutorialUIObj;
    private TutorialUI tutorialUI;

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
    [SerializeField] private GameObject multiBossFightUIObj;
    private MultiBossFightUI multiBossFightUI;

    [Header("Die UI")]
    [SerializeField] private DieUI dieUI;

    private SavepointUIMain savepointUIMain;
    private SavepointUIInventory savepointUIInventory;
    #endregion

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

        #region Initialize UI
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

        multiBossFightUIObj.SetActive(false);
        multiBossFightUI = multiBossFightUIObj.GetComponent<MultiBossFightUI>();

        pickUpItemUIObj.SetActive(false);
        pickUpItemUI = pickUpItemUIObj.GetComponent<PickupItemUI>();

        tutorialUIObj.SetActive(false);
        tutorialUI = tutorialUIObj.GetComponent<TutorialUI>();

        shopUIObj.SetActive(false);

        dieUI.gameObject.SetActive(false);

        loadingObj.SetActive(true);

        savePointNames = new();
        savepoints = new();

        loadingBar.Init(1f);
        #endregion
    }

    // Event subscription
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
        LoadSceneManager.Instance.OnLoadingSingleProgress += HandleLoadingSingleProgress;
    }

    // Event unsubscription
    private void OnDisable()
    {
        GameManager.Instance.OnChangeSceneGoUp -= HandleChangeSceneGoUp;
        GameManager.Instance.OnChangeSceneGoDown -= HandleChangeSceneGoDown;
        GameManager.Instance.OnChangeSceneGoLeft -= HandleChangeSceneGoLeft;
        GameManager.Instance.OnChangeSceneGoRight -= HandleChangeSceneGoRight;
        GameManager.Instance.OnChangeSceneFinished -= HandleChangeSceneFinish;
        GameManager.Instance.OnSavepointInteracted -= HandleSavePointInteraction;

        DataPersistenceManager.Instance.OnSave -= HandleSave;
        LoadSceneManager.Instance.OnLoadingSingleProgress -= HandleLoadingSingleProgress;

    }

    private void Update()
    {
        // inputSystemUIInputModule and the player input compnent can work with each other, so using inputSystemUIInputModule here.
        // 
        // The player can open the pause menu by pressing the cancel button.
        // The player can close menus automatically by pressing the cancel button again.
        if (inputSystemUIInputModule.cancel.action.triggered)
        {
            if(loadingObj.activeInHierarchy || dieUI.gameObject.activeInHierarchy)
            {
                return;
            }

            if (!pauseUIObj.activeInHierarchy &&
                !savepointUIObj.activeInHierarchy &&
                !pickUpItemUIObj.activeInHierarchy &&
                !shopUIObj.activeInHierarchy && 
                !tutorialUIObj.activeInHierarchy)
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
            else if (tutorialUIObj.activeInHierarchy)
            {
                tutorialUI.Deactivate();
            }
        }

        // Pressing "\" to turn off the UI.
        // Mainly used for recording videos and taking screenshots.
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

    public void ActiveTutorialUI(VideoClip clip, LocalizedString title, LocalizedString description)
    {
        tutorialUI.Activate(clip, title, description);
    }

    public void DeactiveTutorialUI()
    {
        tutorialUI.Deactivate();
    }

    public void ActiveDieUI()
    {
        dieUI.Activate();
    }

    public void ActiveBossUI(BossBase bossBase)
    {
        bossFightUI.Active(bossBase);
    }

    public void ActiveMultiBossUI(BossBase bossBase1, BossBase bossBase2)
    {
        multiBossFightUI.Active(bossBase1, bossBase2);
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

    #region Change Scene

    public void HandleLoadingSingleProgress(float progress)
    {
        loadingBar.UpdateHealthBar(1f - progress);

        if (progress >= 1f)
            HandleChangeSceneFinish();
    }


    private void HandleChangeSceneGoUp()
    {
        changeSceneUI.SetActive(true);
        if (bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();

        if (multiBossFightUIObj.activeInHierarchy)
            multiBossFightUI.Deactive();
        changeSceneAnimator.SetTrigger("toUp");
    }

    private void HandleChangeSceneGoDown()
    {
        changeSceneUI.SetActive(true);
        if (bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();

        if (multiBossFightUIObj.activeInHierarchy)
            multiBossFightUI.Deactive();
        changeSceneAnimator.SetTrigger("toDown");
    }

    private void HandleChangeSceneGoLeft()
    {
        changeSceneUI.SetActive(true);
        if (bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();

        if (multiBossFightUIObj.activeInHierarchy)
            multiBossFightUI.Deactive();
        changeSceneAnimator.SetTrigger("toLeft");
    }
    public void HandleChangeSceneGoRight()
    {
        changeSceneUI.SetActive(true);
        if(bossFightUIObj.activeInHierarchy)
            bossFightUI.Deactive();

        if (multiBossFightUIObj.activeInHierarchy)
            multiBossFightUI.Deactive();
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
