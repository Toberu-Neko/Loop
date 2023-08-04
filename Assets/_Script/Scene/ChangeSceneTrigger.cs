using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object leftScene;
    [SerializeField] private UnityEngine.Object rightScene;
    [SerializeField] private Collider2D col;

    private bool isUnloaded = false;

    public event Action OnChangeSceneGoLeft;
    public event Action OnChangeSceneGoRight;

    private void Start()
    {
        GameManager.Instance.RegisterChangeSceneTrigger(this);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - col.bounds.center).normalized;
            if(exitDirection.x > 0 && SceneManager.GetSceneByName(leftScene.name).isLoaded)
            {
                Debug.Log("TO right");

                if (!isUnloaded)
                {
                    isUnloaded = true;
                    SceneManager.UnloadSceneAsync(leftScene.name);
                    OnChangeSceneGoRight?.Invoke();
                }
            }
            else if(SceneManager.GetSceneByName(rightScene.name).isLoaded)
            {
                Debug.Log("TO left");

                if (!isUnloaded)
                {
                    isUnloaded = true;
                    SceneManager.UnloadSceneAsync(rightScene.name);
                    OnChangeSceneGoLeft?.Invoke();
                }
            }
            isUnloaded = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<BoxCollider2D>(out var boxCollider))
            return;

        Gizmos.color = Color.red;

        Bounds bounds = boxCollider.bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
