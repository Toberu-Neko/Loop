using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private bool isLoadedThisFram = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !SceneManager.GetSceneByName(sceneName).isLoaded && !isLoadedThisFram)
        {
            isLoadedThisFram = true;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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
