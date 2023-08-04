using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSingle : MonoBehaviour
{
    [SerializeField] private Object scene;

    public void LoadScene()
    {
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene(scene.name);
    }
}
