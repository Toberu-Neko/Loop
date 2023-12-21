using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    [field: SerializeField] public GameObject LoadingObj { get; set; }
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

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
        CurrentSceneName = sceneName;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        LoadingObj.SetActive(true);
        ObjectPoolManager.ReturnAllObjectsToPool();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("LoadSceneAsync " + progress);
            yield return null;
        }
    }
}
