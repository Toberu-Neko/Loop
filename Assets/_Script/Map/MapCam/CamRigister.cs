using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRigister : MonoBehaviour
{
    [SerializeField] private bool focusOnPlayer = true;
    private void OnEnable()
    {
        CamSwitch.RegisterCam(GetComponent<Cinemachine.CinemachineVirtualCamera>());

        if (focusOnPlayer)
        {
            // GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = GameObject.Find("CameraFollowObject").transform;
            GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = GameObject.Find("Player/Misc/LookAt").transform;
        }
    }
    private void OnDisable()
    {
        CamSwitch.UnregisterCam(GetComponent<Cinemachine.CinemachineVirtualCamera>());
    }
}
