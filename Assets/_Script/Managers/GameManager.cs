using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private UI_InputManager inputManager;

    public bool IsPaused { get; private set; }

    public event Action OnGamePaused;
    public event Action OnGameResumed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            IsPaused = false;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        inputManager = GetComponent<UI_InputManager>();
    }

    private void Update()
    {
        if (inputManager.ESCInput)
        {
            if (IsPaused)
            {
                ResumeGame();
                OnGameResumed?.Invoke();
            }
            else
            {
                PauseGame();
                OnGamePaused?.Invoke();
            }
            inputManager.UseESCInput();
        }
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
}
