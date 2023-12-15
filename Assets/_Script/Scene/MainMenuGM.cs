using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class MainMenuGM : MonoBehaviour
{
    [SerializeField] private GameObject loadingObj;

    public void Start()
    {
        LoadSceneManager.Instance.LoadingObj = loadingObj;
    }
}
