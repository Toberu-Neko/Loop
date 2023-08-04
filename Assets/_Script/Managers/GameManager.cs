using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    public bool TimeStopEnemy { get; private set; }
    public event Action OnAllTimeStopEnemy;
    public event Action OnAllTimeStartEnemy;

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

    public void SetTimeStopEnemyTrue()
    {
        TimeStopEnemy = true;
        OnAllTimeStopEnemy?.Invoke();
    }

    public void SetTimeStopEnemyFalse()
    {
        TimeStopEnemy = false;
        OnAllTimeStartEnemy?.Invoke();
    }

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

}
