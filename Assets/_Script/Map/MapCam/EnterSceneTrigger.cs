using System;
using UnityEngine;

public class EnterSceneTrigger : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamManager.Instance.SwitchCamera(cam);
            GameManager.Instance.HandleChangeSceneFinished();
        }
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<BoxCollider2D>(out var boxCollider))
            return;

        Gizmos.color = Color.gray;

        Bounds bounds = boxCollider.bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
