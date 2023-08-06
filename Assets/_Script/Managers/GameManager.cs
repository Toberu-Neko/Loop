using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    public bool TimeStopAll { get; private set; } = false;
    public bool TimeSlowAll { get; private set; } = false;

    public event Action OnAllTimeStopEnd;
    public event Action OnAllTimeStopStart;

    public float TimeSlowMultiplier { get; private set; } = 0.2f;
    public event Action OnAllTimeSlowStart;
    public event Action OnAllTimeSlowEnd;

    public event Action OnGamePaused;
    public event Action OnGameResumed;


    public event Action OnChangeSceneGoLeft;
    public event Action OnChangeSceneGoRight;
    public event Action OnChangeSceneFinished;
    private List<ChangeSceneTrigger> changeSceneTriggers;
    private List<EnterSceneTrigger> enterSceneTriggers;

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

        changeSceneTriggers = new List<ChangeSceneTrigger>();
        enterSceneTriggers = new List<EnterSceneTrigger>();
    }
    private void OnDisable()
    {
        foreach (var trigger in changeSceneTriggers)
        {
            trigger.OnChangeSceneGoLeft -= HandleChangeSceneGoLeft;
            trigger.OnChangeSceneGoRight -= HandleChangeSceneGoRight;
        }

        foreach (var trigger in enterSceneTriggers)
        {
            trigger.OnChangeSceneFinished -= HandleChangeSceneFinished;
        }

        changeSceneTriggers.Clear();
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        OnGameResumed?.Invoke();
    }

    public void StartAllTimeSlow(float duration, float multiplier)
    {
        if(TimeSlowAll)
        {
            Debug.LogError("TimeSlowAll is already true");
        }
        TimeSlowMultiplier = multiplier;
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

    #region ChangeScene

    public void RegisterChangeSceneTrigger(ChangeSceneTrigger trigger)
    {
        changeSceneTriggers.Add(trigger);
        trigger.OnChangeSceneGoLeft += HandleChangeSceneGoLeft;
        trigger.OnChangeSceneGoRight += HandleChangeSceneGoRight;
    }

    public void RegisterEnterSceneTrigger(EnterSceneTrigger trigger)
    {
        enterSceneTriggers.Add(trigger);
        trigger.OnChangeSceneFinished += HandleChangeSceneFinished;
    }

    private void HandleChangeSceneGoLeft()
    {
        OnChangeSceneGoLeft?.Invoke();
    }

    private void HandleChangeSceneGoRight()
    {
        OnChangeSceneGoRight?.Invoke();
    }

    private void HandleChangeSceneFinished()
    {
        OnChangeSceneFinished?.Invoke();
    }
    #endregion
}
