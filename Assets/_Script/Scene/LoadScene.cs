using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private SceneReference scene;
    private bool isLoadedThisFram = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !SceneManager.GetSceneByName(scene.Name).isLoaded && !isLoadedThisFram)
        {
            isLoadedThisFram = true;
            SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLoadedThisFram = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<CompositeCollider2D>(out var compositeCollider))
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < compositeCollider.pathCount; i++)
        {
            var path = new List<Vector2>();
            compositeCollider.GetPath(i, path);
            for (int j = 0; j < path.Count; j++)
            {
                var worldPos = transform.TransformPoint(path[j]);
                Gizmos.DrawSphere(worldPos, 0.05f);
                if (j > 0)
                {
                    var prevWorldPos = transform.TransformPoint(path[j - 1]);
                    Gizmos.DrawLine(worldPos, prevWorldPos);
                }
            }
        }
    }
}
