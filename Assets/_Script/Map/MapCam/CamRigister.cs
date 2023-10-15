using UnityEngine;

public class CamRigister : MonoBehaviour
{
    [SerializeField] private bool focusOnPlayer = true;
    [SerializeField] private Transform lookAt;

    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;

    private void Start()
    {
        if (focusOnPlayer)
        {
            cam.Follow = CamManager.Instance.PlayerLookat;
        }
        else
        {
            cam.Follow = lookAt;
        }
        cam.enabled = false;
    }
}
