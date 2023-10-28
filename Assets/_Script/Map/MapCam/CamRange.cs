using System.Collections.Generic;
using UnityEngine;

public class CamRange : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;
    [SerializeField] private bool autoChangeCam = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!autoChangeCam)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            CamManager.Instance.SwitchCamera(cam);
            GameManager.Instance.HandleChangeSceneFinished();
        }
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<CompositeCollider2D>(out var compositeCollider))
            return;

        Gizmos.color = Color.cyan;
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
