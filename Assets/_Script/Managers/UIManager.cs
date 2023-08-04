using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject pauseMainUI;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameManager.OnGamePaused += OpenPauseMainUI;
        gameManager.OnGameResumed += ClosePauseMainUI;

        ClosePauseMainUI();
    }

    private void OnDisable()
    {
        gameManager.OnGamePaused -= OpenPauseMainUI;
        gameManager.OnGameResumed -= ClosePauseMainUI;
    }

    private void OpenPauseMainUI()
    {
        pauseMainUI.SetActive(true);
    }

    private void ClosePauseMainUI()
    {
        pauseMainUI.SetActive(false);
    }
}
