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
}
