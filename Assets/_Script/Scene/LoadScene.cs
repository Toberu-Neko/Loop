using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Object scene;
    private bool isLoadedThisFram = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !SceneManager.GetSceneByName(scene.name).isLoaded && !isLoadedThisFram)
        {
            isLoadedThisFram = true;
            SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Additive);
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
