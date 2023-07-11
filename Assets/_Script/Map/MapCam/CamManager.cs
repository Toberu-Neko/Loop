using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CamManager
{
    private static List<CinemachineVirtualCamera> cams = new();
    public static CinemachineVirtualCamera activatedCam = null;
    public static void RegisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Add(vcam);
        vcam.enabled = false;
    }
    public static void UnregisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Remove(vcam);
    }
}
