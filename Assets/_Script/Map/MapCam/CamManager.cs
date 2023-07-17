using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private List<CinemachineVirtualCamera> cams = new();
    [HideInInspector] public CinemachineVirtualCamera activatedCam;
    public void RegisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Add(vcam);
        vcam.enabled = false;
    }
    public void UnregisterCam(CinemachineVirtualCamera vcam)
    {
        cams.Remove(vcam);
    }
    public void SwitchCamera(CinemachineVirtualCamera vcam)
    {
        activatedCam = vcam;
        vcam.enabled = true;

        framingTransposer = activatedCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }

    public static CamManager instance;

    private CinemachineFramingTransposer framingTransposer;

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;

    private Vector2 startingTrackedObjectOffset;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Debug.LogError("There is more than one CamManager in the scene!");
    }

    #region Pan Camera
    public void PanCameraOnTrigger(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPoint)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPoint));
    }

    public IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPoint)
    {
        Vector2 endPosition = Vector2.zero;
        Vector2 startPosition = Vector2.zero;

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

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera cameraLeft, CinemachineVirtualCamera cameraRight, Vector2 triggerExitDirection)
    {
        if (activatedCam == cameraLeft && triggerExitDirection.x > 0f)
        {
            cameraLeft.enabled = false;
            SwitchCamera(cameraRight);
        }
        else if (activatedCam == cameraRight && triggerExitDirection.x < 0f)
        {
            cameraRight.enabled = false;
            SwitchCamera(cameraLeft);
        }
    }
    #endregion
}
