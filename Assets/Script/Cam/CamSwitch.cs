using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    private static List<CinemachineVirtualCamera> cams = new();
    public static CinemachineVirtualCamera activatedCam = null;

    private void SwitchCamera(CinemachineVirtualCamera vcam)
    {
        vcam.Priority = 10;
        activatedCam = vcam;

        foreach(CinemachineVirtualCamera cam in cams) 
        {
            if(cam != vcam && cam.Priority != 0)
            {
                cam.Priority = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.CompareTag("Player"))
        {
            SwitchCamera(cam);
        }
    }
    public static void RegisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Add(vcam);
    }
    public static void UnregisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Remove(vcam);
    }


    private void OnDrawGizmos()
    {
        if (!TryGetComponent<PolygonCollider2D>(out var polygonCollider))
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            var path = polygonCollider.GetPath(i);
            for (int j = 0; j < path.Length; j++)
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
