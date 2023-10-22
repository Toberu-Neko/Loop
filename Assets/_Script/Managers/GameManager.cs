using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Pause, TimeStop, TimeSlow, ChangeScene, OnSavepoint

    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    public bool TimeStopAll { get; private set; } = false;
    public bool TimeSlowAll { get; private set; } = false;

    public event Action<string> OnSavepointInteracted;

    public event Action OnAllTimeStopEnd;
    public event Action OnAllTimeStopStart;

    [field: SerializeField, Range(0.001f, 1f)] public float TimeSlowMultiplier { get; private set; } = 0.2f;
    public event Action OnAllTimeSlowStart;
    public event Action OnAllTimeSlowEnd;

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
        if (Savepoints.ContainsKey(savePoint.SavePointName))
        {
            Debug.LogError("Savepoint name already exists! Check: " + savePoint.SavePointName);
            return;
        }

        Savepoints.Add(savePoint.SavePointName, savePoint);

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

    public void HandleSavePointInteraction(string savepointName)
    {
        OnSavepointInteracted?.Invoke(savepointName);

        DataPersistenceManager.Instance.SaveGame();
        EnemyManager.Instance.ResetTempData();
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
