using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRigister : MonoBehaviour
{
    [SerializeField] private bool focusOnPlayer = true;

    private Cinemachine.CinemachineVirtualCamera cam;
    private Transform player;

    private void Awake()
    {
        cam = GetComponent<Cinemachine.CinemachineVirtualCamera>();

        // player = GameObject.Find("CameraFollowObject").transform;
        player = GameObject.Find("Player/Misc/LookAt").transform;

    }
    private void OnEnable()
    {
        CamManager.RegisterCam(cam);

        if (focusOnPlayer)
        {
            cam.Follow = player;
        }
    }
    private void OnDisable()
    {
        CamManager.UnregisterCam(cam);
    }
}
