using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRigister : MonoBehaviour
{
    private void OnEnable()
    {
        CamSwitch.RegisterCam(GetComponent<Cinemachine.CinemachineVirtualCamera>());
    }
    private void OnDisable()
    {
        CamSwitch.UnregisterCam(GetComponent<Cinemachine.CinemachineVirtualCamera>());
    }
}
