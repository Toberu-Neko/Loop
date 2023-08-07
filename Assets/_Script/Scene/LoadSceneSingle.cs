using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSingle : MonoBehaviour
{
    [SerializeField] private Object scene;

    public void LoadScene()
    {
        Debug.Log("Loading scene: " + scene.name);
        SceneManager.LoadScene(scene.name);
    }
}
