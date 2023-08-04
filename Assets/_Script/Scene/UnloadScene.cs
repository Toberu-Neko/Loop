using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    [SerializeField] private Object scene;
    private bool isLoadedThisFram = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SceneManager.GetSceneByName(scene.name).isLoaded && !isLoadedThisFram)
        {
            isLoadedThisFram = true;
            SceneManager.UnloadSceneAsync(scene.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLoadedThisFram = false;
        }
    }
}
