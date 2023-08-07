using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class LoadSceneSingle : MonoBehaviour
{
    [SerializeField] private SceneReference scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(scene.Name);
    }
}
