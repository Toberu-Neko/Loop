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

        gameManager.OnChangeSceneGoLeft += HandleChangeSceneGoLeft;
        gameManager.OnChangeSceneGoRight += HandleChangeSceneGoRight;
        gameManager.OnChangeSceneFinished += HandleChangeSceneFinish;
    }

    private void OnDisable()
    {
        gameManager.OnGamePaused -= OpenPauseMainUI;
        gameManager.OnGameResumed -= ClosePauseMainUI;

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

    private void HandleChangeSceneGoLeft()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetBool("finishLoading", false);
        changeSceneAnimator.SetTrigger("toLeft");
        Debug.Log("ChangeSceneUI Open Go Left");
    }
    private void HandleChangeSceneGoRight()
    {
        changeSceneUI.SetActive(true);
        changeSceneAnimator.SetBool("finishLoading", false);
        changeSceneAnimator.SetTrigger("toRight");
        Debug.Log("ChangeSceneUI Open Go Right");
    }
    private void HandleChangeSceneFinish()
    {
        changeSceneAnimator.SetBool("finishLoading", true);
    }
    private void OnFinishChangeSceneAnimation()
    {
        changeSceneUI.SetActive(false);
    }
}
