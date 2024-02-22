using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Pause, TimeStop, TimeSlow, ChangeScene, OnSavepoint

    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    public bool TimeStopAll { get; private set; } = false;
    public bool TimeSlowAll { get; private set; } = false;

    public event Action<string, LocalizedString> OnSavepointInteracted;

    public event Action OnAllTimeStopEnd;
    public event Action OnAllTimeStopStart;

    [field: SerializeField, Range(0.001f, 1f)] public float TimeSlowMultiplier { get; private set; } = 0.2f;
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [SerializeField] private GameObject loadingObj;
    public event Action OnAllTimeSlowStart;
    public event Action OnAllTimeSlowEnd;


    [SerializeField] private GameObject globalVolumeNight;
    [SerializeField] private GameObject globalVolumeDay;

    public Dictionary<string, Savepoint> Savepoints { get; private set; }

    #region Change Scene Variables

    public event Action OnChangeSceneGoLeft;
    public event Action OnChangeSceneGoRight;
    public event Action OnChangeSceneGoUp;
    public event Action OnChangeSceneGoDown;
    public event Action OnChangeSceneFinished;

    #endregion

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
        IsPaused = false;
        TimeStopAll = false;
        TimeSlowAll = false;

        Savepoints = new();

        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MultiSceneBase" || scene.name == "MainMenu")
        {
            // Debug.LogWarning("MultiSceneBase or MainMenu loaded");
            return;
        }

        if ((scene.name == "Level1-0" || scene.name == "Level1-1" || scene.name == "Level1-2-1") && mode == LoadSceneMode.Additive)
        {
            // Debug.LogWarning("Level1-0 or Level1-1 loaded");
            globalVolumeDay.SetActive(false);
            globalVolumeNight.SetActive(true);
        }
        else
        {
            // Debug.LogWarning("Other scene loaded");
            globalVolumeDay.SetActive(true);
            globalVolumeNight.SetActive(false);
        }
    }

    private void Start()
    {
        LoadSceneManager.Instance.LoadingObj = loadingObj;
    }


    private void OnDisable()
    {
        foreach (var savepoint in Savepoints)
        {
            savepoint.Value.OnSavePointInteract -= HandleSavePointInteraction;
        }
        Savepoints.Clear();
    }

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

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void HandleSavePointInteraction(string savepointID, LocalizedString savepointName)
    {
        OnSavepointInteracted?.Invoke(savepointID, savepointName);


        DataPersistenceManager.Instance.SaveGame();

        EnemyManager.Instance.ResetTempData();
    }

    public void HandleShopInteraction(string shopID, LocalizedString shopName)
    {
        // OnShopInteracted?.Invoke(shopID, shopName);


    }

    public Vector3 GetSavepointTeleportPos(string savepointName)
    {
        Savepoints.TryGetValue(savepointName, out Savepoint savepoint);
        return savepoint.TeleportTransform.position;
    }

    #region Time

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
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void HandleChangeSceneFinished()
    {
        OnChangeSceneFinished?.Invoke();
    }

    public enum ChangeSceneDir
    {
        Right, Left, Up, Down
    }
    #endregion
}
