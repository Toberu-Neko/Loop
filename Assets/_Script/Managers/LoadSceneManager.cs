using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    public GameObject LoadingObj { get; set; }
    public string CurrentSceneName { get; set; }

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

    public void LoadSceneAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncAdditive(sceneName));
        CurrentSceneName = sceneName;
    }

    public void LoadSceneSingle(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncSingle(sceneName));
        CurrentSceneName = sceneName;
    }

    private IEnumerator LoadSceneAsyncSingle(string sceneName)
    {
        LoadingObj.SetActive(true);
        ObjectPoolManager.ReturnAllObjectsToPool();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("LoadSceneAsyncSingle " + progress);
            yield return null;
        }
    }

    private IEnumerator LoadSceneAsyncAdditive(string sceneName)
    {
        // LoadingObj.SetActive(true);
        // ObjectPoolManager.ReturnAllObjectsToPool();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("LoadSceneAsyncAdditive " + progress);
            yield return null;
        }
    }

}
