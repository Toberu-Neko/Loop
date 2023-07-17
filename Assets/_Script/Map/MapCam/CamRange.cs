using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamRange : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;


    /* private void OnTriggerEnter2D(Collider2D trigger)
     {
         if(trigger.CompareTag("Player"))
         {
             CamManager.instance.SwitchCamera(cam);
         }
     }
     private void OnTriggerExit2D(Collider2D trigger)
     {
         if (trigger.CompareTag("Player"))
         {
             // cam.enabled = false;

             //TODO: �����{���X�n����L����h�A����P���Collider�ۦP
         }
     }*/



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
