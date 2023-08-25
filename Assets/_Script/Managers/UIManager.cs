using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }
    private GameManager gameManager;

    [SerializeField] private GameObject pauseMainUI;
    [SerializeField] private GameObject changeSceneUI;
    [SerializeField] private Animator changeSceneAnimator;

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

        pauseMainUI.SetActive(false);
        changeSceneUI.SetActive(false);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.OnGamePaused += OpenPauseMainUI;
        gameManager.OnGameResumed += ClosePauseMainUI;

        gameManager.OnChangeSceneGoUp += HandleChangeSceneGoUp;
        gameManager.OnChangeSceneGoDown += HandleChangeSceneGoDown;
        gameManager.OnChangeSceneGoLeft += HandleChangeSceneGoLeft;
        gameManager.OnChangeSceneGoRight += HandleChangeSceneGoRight;
        gameManager.OnChangeSceneFinished += HandleChangeSceneFinish;
    }

    private void OnDisable()
    {
        gameManager.OnGamePaused -= OpenPauseMainUI;
        gameManager.OnGameResumed -= ClosePauseMainUI;

        gameManager.OnChangeSceneGoUp -= HandleChangeSceneGoUp;
        gameManager.OnChangeSceneGoDown -= HandleChangeSceneGoDown;
        gameManager.OnChangeSceneGoLeft -= HandleChangeSceneGoLeft;
        gameManager.OnChangeSceneGoRight -= HandleChangeSceneGoRight;
        gameManager.OnChangeSceneFinished -= HandleChangeSceneFinish;
    }

    private void OpenPauseMainUI()
    {
        pauseMainUI.SetActive(true);
    }

    private void ClosePauseMainUI()
    {
        pauseMainUI.SetActive(false);
    }

    private void HandleChangeSceneGoUp()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toUp");
    }

    private void HandleChangeSceneGoDown()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toDown");
    }

    private void HandleChangeSceneGoLeft()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toLeft");
    }
    private void HandleChangeSceneGoRight()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetTrigger("toRight");
    }
    private void HandleChangeSceneFinish()
    {
        if(changeSceneUI.activeInHierarchy)
            changeSceneAnimator.SetBool("finishLoading", true);
    }
}
