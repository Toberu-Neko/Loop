using Eflatun.SceneReference;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager class is a singleton class that manages the game's global settings.
/// Like pausing the game, time manipulation, changing scenes, global volume, and interacting with savepoints.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    public bool TimeStopAll { get; private set; } = false;
    public bool TimeSlowAll { get; private set; } = false;
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [SerializeField] private GameObject loadingObj;
    [SerializeField] private VirtualMouseUI virtualMouseUI;

    public event Action<string, LocalizedString> OnSavepointInteracted;

    public event Action OnAllTimeStopEnd;
    public event Action OnAllTimeStopStart;

    [field: SerializeField, Range(0.001f, 1f)] public float TimeSlowMultiplier { get; private set; } = 0.2f;

    public event Action OnAllTimeSlowStart;
    public event Action OnAllTimeSlowEnd;

    #region Global Volume Variables
    [Header("Global Volume")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile dayVolumeProfile;
    [SerializeField] private VolumeProfile nightVolumeProfile;
    [SerializeField] private VolumeProfile inTempleVolumeProfile;

    [SerializeField] private Color orgVigColor;
    [SerializeField] private Color hurtVigColor;
    [SerializeField] private float hurtVigIntensityHigh;
    [SerializeField] private float hurtVigIntensityLow;
    [SerializeField] private float orgIntensity;

    private bool playerInDanger;

    private Vignette dayVignette;
    private Vignette nightVignette;
    private Vignette inTempleVignette;
    #endregion

    public Dictionary<string, Savepoint> Savepoints { get; private set; }

    [SerializeField] private SceneReference startAnimScene;

    #region Change Scene Variables

    public event Action OnChangeSceneGoLeft;
    public event Action OnChangeSceneGoRight;
    public event Action OnChangeSceneGoUp;
    public event Action OnChangeSceneGoDown;
    public event Action OnChangeSceneFinished;

    #endregion

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += HandleSceneLoadedForGlobalVolume;

        #region Initialize Variables
        IsPaused = false;
        TimeStopAll = false;
        TimeSlowAll = false;
        playerInDanger = false;

        Savepoints = new();

        dayVolumeProfile.TryGet(out dayVignette);
        nightVolumeProfile.TryGet(out nightVignette);
        inTempleVolumeProfile.TryGet(out inTempleVignette);
        globalVolume.gameObject.SetActive(true);

        dayVignette.color.value = orgVigColor;
        dayVignette.intensity.value = orgIntensity;

        nightVignette.color.value = orgVigColor;
        nightVignette.intensity.value = orgIntensity;

        inTempleVignette.color.value = orgVigColor;
        inTempleVignette.intensity.value = orgIntensity;
        #endregion
    }

    private void Start()
    {
        LoadSceneManager.Instance.LoadingObj = loadingObj;
        LoadSceneManager.Instance.OnLoadingAdditiveProgress += HandleLoadingAdditiveProgress;

        DataPersistenceManager.Instance.LoadOptionData();
    }

    private void Update()
    {
        // Set hurt vignette effect when player is in danger.
        if (playerInDanger)
        {
            if(globalVolume.profile == dayVolumeProfile)
            {
                dayVignette.color.value = Color.Lerp(dayVignette.color.value, hurtVigColor, Time.deltaTime * 2f);
                dayVignette.intensity.value = Mathf.Lerp(dayVignette.intensity.value, hurtVigIntensityHigh, Time.deltaTime * 2f);
            }
            else
            {
                nightVignette.color.value = Color.Lerp(nightVignette.color.value, hurtVigColor, Time.deltaTime * 2f);
                nightVignette.intensity.value = Mathf.Lerp(nightVignette.intensity.value, hurtVigIntensityHigh, Time.deltaTime * 2f);
            }
        }
        else
        {
            if (globalVolume.profile == dayVolumeProfile)
            {
                dayVignette.color.value = Color.Lerp(dayVignette.color.value, orgVigColor, Time.deltaTime * 2f);
                dayVignette.intensity.value = Mathf.Lerp(dayVignette.intensity.value, orgIntensity, Time.deltaTime * 2f);
            }
            else
            {
                nightVignette.color.value = Color.Lerp(nightVignette.color.value, orgVigColor, Time.deltaTime * 2f);
                nightVignette.intensity.value = Mathf.Lerp(nightVignette.intensity.value, orgIntensity, Time.deltaTime * 2f);
            }
        }
    }

    // Unsubscribe events and clear the savepoints dictionary when the GameManager is disabled.
    private void OnDisable()
    {
        foreach (var savepoint in Savepoints)
        {
            savepoint.Value.OnSavePointInteract -= HandleSavePointInteraction;
        }
        Savepoints.Clear();
        LoadSceneManager.Instance.OnLoadingAdditiveProgress -= HandleLoadingAdditiveProgress;
        SceneManager.sceneLoaded -= HandleSceneLoadedForGlobalVolume;

        dayVignette.color.value = orgVigColor;
        dayVignette.intensity.value = orgIntensity;

        nightVignette.color.value = orgVigColor;
        nightVignette.intensity.value = orgIntensity;
    }
    #endregion

    #region Global Volume Functions
    // Change the global volume profile based on the scene loaded.
    private void HandleSceneLoadedForGlobalVolume(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiSceneBase" || scene.name == "MainMenu")
        {
            return;
        }

        if ((scene.name == "Level1-0" || scene.name == "Level1-1" || scene.name == "Level1-2-1") && mode == LoadSceneMode.Additive)
        {
            if (globalVolume.profile != nightVolumeProfile)
            {
                Debug.Log("Change to night");
                globalVolume.profile = nightVolumeProfile;
            }
        }
        else if ((scene.name == "Level1-3" || scene.name == "Level1-3-1" || scene.name == "Level1-4(BOSS)") && mode == LoadSceneMode.Additive)
        {
            if (globalVolume.profile != dayVolumeProfile)
            {
                Debug.Log("Change to day");
                globalVolume.profile = dayVolumeProfile;
            }
        }
        else
        {
            if (globalVolume.profile != inTempleVolumeProfile)
            {
                Debug.Log("Change to temple");
                globalVolume.profile = inTempleVolumeProfile;
            }
        }
    }

    public void SetPlayerDanger(bool vol)
    {
        playerInDanger = vol;
    }
    #endregion

    public void RegisterSavePoints(Savepoint savePoint)
    {
        if (Savepoints.ContainsKey(savePoint.SavePointData.savepointID))
        {
            Debug.LogError("Savepoint name already exists! Check: " + savePoint.SavePointData.savepointID);
            return;
        }

        Savepoints.Add(savePoint.SavePointData.savepointID, savePoint);

        savePoint.OnSavePointInteract += HandleSavePointInteraction;
    }

    /// <summary>
    /// Use this to pause the game.
    /// </summary>
    public void PauseGame()
    {
        IsPaused = true;
        PlayerInput.enabled = false;
        virtualMouseUI.SetIsUIOpen(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Use this to resume the game.
    /// </summary>
    public void ResumeGame()
    {
        IsPaused = false;
        PlayerInput.enabled = true;
        virtualMouseUI.SetIsUIOpen(false);
        Time.timeScale = 1f;
    }

    public void HandleSavePointInteraction(string savepointID, LocalizedString savepointName)
    {
        OnSavepointInteracted?.Invoke(savepointID, savepointName);

        DataPersistenceManager.Instance.SaveGame();
        EnemyManager.Instance.ResetTempData();
    }

    public Vector3 GetSavepointTeleportPos(string savepointName)
    {
        Savepoints.TryGetValue(savepointName, out Savepoint savepoint);
        return savepoint.TeleportTransform.position;
    }

    // Reset the temporary save and load the start animation scene, for the first boss fight.
    public void LoadStartAnimScene()
    {
        DataPersistenceManager.Instance.GameData.lastInteractedSavepointID = "Spawn";
        DataPersistenceManager.Instance.GameData.firstTimePlaying = false;

        DataPersistenceManager.Instance.GameData.equipedWeapon[0] = WeaponType.None;
        DataPersistenceManager.Instance.GameData.equipedWeapon[1] = WeaponType.None;

        DataPersistenceManager.Instance.SaveGame(true);
        gameObject.SetActive(false);
        LoadSceneManager.Instance.LoadSceneSingle(startAnimScene.Name);
    }

    #region Time
    /// <summary>
    /// For global time slow skill.
    /// </summary>
    /// <param name="duration"></param>
    public void StartAllTimeSlow(float duration)
    {
        if(TimeSlowAll)
        {
            Debug.LogError("TimeSlowAll is already true");
        }
        TimeSlowAll = true;
        OnAllTimeSlowStart?.Invoke();

        CancelInvoke(nameof(EndAllTimeSlow));
        Invoke(nameof(EndAllTimeSlow), duration);
    }

    private void EndAllTimeSlow()
    {
        TimeSlowAll = false;
        OnAllTimeSlowEnd?.Invoke();
    }

    public void SetTimeStopEnemyTrue()
    {
        TimeStopAll = true;
        OnAllTimeStopStart?.Invoke();
    }

    public void SetTimeStopEnemyFalse()
    {
        TimeStopAll = false;
        OnAllTimeStopEnd?.Invoke();
    }
    #endregion

    #region ChangeScene

    public void HandleChangeScene(string sceneName, ChangeSceneDir dir = ChangeSceneDir.Right)
    {
        switch (dir)
        {
            case ChangeSceneDir.Right:
                OnChangeSceneGoRight?.Invoke();
                break;
            case ChangeSceneDir.Left:
                OnChangeSceneGoLeft?.Invoke();
                break;
            case ChangeSceneDir.Up:
                OnChangeSceneGoUp?.Invoke();
                break;
            case ChangeSceneDir.Down:
                OnChangeSceneGoDown?.Invoke();
                break;
            default:
                OnChangeSceneGoRight?.Invoke();
                break;
        }
        // DataPersistenceManager.Instance.CheckIfShouldSaveOnLoad();
        LoadSceneManager.Instance.UnloadSceneAdditive(sceneName);
    }

    private bool enteredScene = false;
    public void HandleChangeSceneFinished()
    {
        enteredScene = true;
    }

    private void HandleLoadingAdditiveProgress(float progress)
    {
        if (progress >= 1f)
        {
            if (enteredScene)
            {
                OnChangeSceneFinished?.Invoke();
                enteredScene = false;
            }
            else
            {
                CancelInvoke(nameof(CheckChangeSceneFinished));
                Invoke(nameof(CheckChangeSceneFinished), Time.fixedDeltaTime);
            }
        }
    }

    private void CheckChangeSceneFinished()
    {
        if (enteredScene)
        {
            OnChangeSceneFinished?.Invoke();
            enteredScene = false;
        }
    }

    public enum ChangeSceneDir
    {
        Right, Left, Up, Down
    }
    #endregion
}
