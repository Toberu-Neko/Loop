using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointActiveCam : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CamManager.instance.SwitchCamera(cam);
    }
}
