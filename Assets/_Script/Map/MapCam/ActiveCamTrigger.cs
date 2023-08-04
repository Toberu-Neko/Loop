using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCamTrigger : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamManager.instance.SwitchCamera(cam);
        }
    }
}
