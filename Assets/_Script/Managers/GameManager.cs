using System;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            IsPaused = false;
            // Debug.LogError("There is more than one GameManager in the scene!");
            return;
        }

        IsPaused = false;
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
}
