using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    public GameObject LoadingObj { get; set; }
    public string CurrentSceneName { get; set; }

    public event Action<float> OnLoadingSingleProgress;
    public event Action<float> OnLoadingAdditiveProgress;
    public event Action<float> OnUnloadingAdditiveProgress;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneSingle(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncSingle(sceneName));
        CurrentSceneName = sceneName;
    }

    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncAdditive(sceneName));
        CurrentSceneName = sceneName;
    }

    public void UnloadSceneAdditive(string sceneName)
    {
        StartCoroutine(UnloadSceneAsuncAdditive(sceneName));
    }

    private IEnumerator LoadSceneAsyncSingle(string sceneName)
    {
        LoadingObj.SetActive(true);
        ObjectPoolManager.ReturnAllObjectsToPool();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadingSingleProgress?.Invoke(progress); // UI_Manager.Instance.HandleLoadingSingleProgress();

            // Debug.Log("LoadSceneAsyncSingle " + progress);
            yield return null;
        }
    }

    private IEnumerator LoadSceneAsyncAdditive(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadingAdditiveProgress?.Invoke(progress);
            // Debug.Log("LoadSceneAsyncAdditive " + progress);
            yield return null;
        }
    }

    private IEnumerator UnloadSceneAsuncAdditive(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            float progress = Mathf.Clamp01(asyncUnload.progress / 0.9f);
            OnUnloadingAdditiveProgress?.Invoke(progress);
            // Debug.Log("UnloadSceneAsuncAdditive " + progress);
            yield return null;
        }

        // Debug.Log("UnloadSceneAsuncAdditive " + sceneName + " is done");
    }

}
