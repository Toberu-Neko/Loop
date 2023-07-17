using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        gameManager.OnGamePaused += OpenPauseMainUI;
        gameManager.OnGameResumed += ClosePauseMainUI;
    }

    private void OpenPauseMainUI()
    {

    }

    private void ClosePauseMainUI()
    {

    }
}
