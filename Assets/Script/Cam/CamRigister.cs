using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRigister : MonoBehaviour
{
    private void OnEnable()
    {
        CamSwitch.RegisterCam(GetComponent<Cinemachine.CinemachineVirtualCamera>());
        GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = GameObject.Find("Player/Misc/LookAt").transform;
    }
    private void OnDisable()
    {
        CamSwitch.UnregisterCam(GetComponent<Cinemachine.CinemachineVirtualCamera>());
    }
}
