using UnityEngine;

public class CamRigister : MonoBehaviour
{
    [SerializeField] private bool focusOnPlayer = true;

    [SerializeField] private Cinemachine.CinemachineVirtualCamera cam;

    private void Start()
    {
        if (focusOnPlayer)
        {
            cam.Follow = CamManager.Instance.PlayerLookat;
        }
        cam.enabled = false;
    }
}
