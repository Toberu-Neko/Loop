using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static CamManager Instance { get; private set;}

    // private List<CinemachineVirtualCamera> cams = new();
    public CinemachineVirtualCamera CurrentCam { get; private set; }
    private CinemachineFramingTransposer framingTransposer;
    
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private float shakeForce = 1f;
    [field: SerializeField] public Transform PlayerLookat { get; private set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;

    private Vector2 startingTrackedObjectOffset;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    /*
    #region Register/Unregister Cams
    public void RegisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Add(vcam);
        vcam.enabled = false;
    }
    public void UnregisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Remove(vcam);
    }
    #endregion
    */
    #region Swap/Switch Cameras

    public void SwitchCamera(CinemachineVirtualCamera vcam)
    {
        CurrentCam = vcam;
        vcam.enabled = true;

        framingTransposer = CurrentCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }

    public void SwapCamera(CinemachineVirtualCamera cameraLeft, CinemachineVirtualCamera cameraRight, Vector2 triggerExitDirection)
    {
        if (CurrentCam == cameraLeft && triggerExitDirection.x > 0f)
        {
            cameraLeft.enabled = false;
            SwitchCamera(cameraRight);
        }
        else if (CurrentCam == cameraRight && triggerExitDirection.x < 0f)
        {
            cameraRight.enabled = false;
            SwitchCamera(cameraLeft);
        }
    }
    #endregion

    #region Pan Camera
    public void PanCameraOnTrigger(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPoint)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPoint));
    }

    public IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPoint)
    {
        Vector2 endPosition = Vector2.zero;
        Vector2 startPosition;

        if (!panToStartingPoint)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPosition = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPosition = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPosition = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPosition = Vector2.right;
                    break;
                default:
                    break;
            }

            endPosition *= panDistance;

            startPosition = startingTrackedObjectOffset;

            endPosition += startPosition;
        }

        else
        {
            startPosition = framingTransposer.m_TrackedObjectOffset;
            endPosition = startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            framingTransposer.m_TrackedObjectOffset = Vector2.Lerp(startPosition, endPosition, elapsedTime / panTime);
            yield return null;
        }
    }
    #endregion

    public void CameraShake()
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }

}
