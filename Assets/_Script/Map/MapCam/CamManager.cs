using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for managing the cameras in the game.
/// </summary>
public class CamManager : MonoBehaviour
{
    public static CamManager Instance { get; private set;}
    public Camera MainCamera { get; private set; }

    // private List<CinemachineVirtualCamera> cams = new();
    private List<CinemachineVirtualCamera> activeCams;
    public CinemachineVirtualCamera CurrentCam { get; private set; }
    private CinemachineFramingTransposer framingTransposer;
    
    private CinemachineImpulseSource impulseSource;
    private bool canShackCamera = true;
    [field: SerializeField] public Transform PlayerLookat { get; private set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;

    private Vector2 startingTrackedObjectOffset;
    private float targetFOV;
    private float orgFOV;

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

        activeCams = new();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        canShackCamera = true;
        MainCamera = Camera.main;
    }

    #region Swap/Switch Cameras

    public void EnterCamBoarder(CinemachineVirtualCamera vcam)
    {
        if(activeCams.Contains(vcam))
        {
            Debug.LogWarning("Trying to enter the same camera");
            return;
        }

        activeCams.Add(vcam);

        if (activeCams.Count == 1)
        {
            SwitchCamera(vcam);
        }
    }

    public void ExitCamBoarder(CinemachineVirtualCamera vcam)
    {
        if (!activeCams.Contains(vcam))
        {
            Debug.LogWarning("Trying to exit a camera that is not active");
            return;
        }

        activeCams.Remove(vcam);
        
        if (activeCams.Count == 1)
        {
            SwitchCamera(activeCams[0]);
        }
    }

    public void SwitchCamera(CinemachineVirtualCamera vcam)
    {
        if(CurrentCam == vcam)
        {
            Debug.LogWarning("Trying to switch to the same camera");
            return;
        }

        if(CurrentCam)
            CurrentCam.enabled = false;

        CurrentCam = vcam;
        CurrentCam.enabled = true;
        activeCams.Clear();
        activeCams.Add(vcam);

        orgFOV = CurrentCam.m_Lens.FieldOfView;
        ChangeFOV();

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

    public void ChangeFOV(float FOV = 0f)
    {
        if (CurrentCam == null)
        {
            Debug.LogWarning("CurrentCam is null");
            return;
        }

        if(FOV == 0f)
        {
            FOV = orgFOV;
        }

        targetFOV = FOV;
        Inv_ChangeFOV();
    }

    private void Inv_ChangeFOV()
    {
        if (CurrentCam == null)
        {
            Debug.LogWarning("CurrentCam is null, in ChangeFOV");
            return;
        }

        CancelInvoke(nameof(Inv_ChangeFOV));
        CurrentCam.m_Lens.FieldOfView = Mathf.Lerp(CurrentCam.m_Lens.FieldOfView, targetFOV, Time.deltaTime * 2f);  
        if (Mathf.Abs(CurrentCam.m_Lens.FieldOfView - targetFOV) > 0.1f)
        {
            Invoke(nameof(Inv_ChangeFOV), Time.deltaTime);
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

    public void CameraShake(float shakeForce = 1f)
    {
        if (canShackCamera)
        {
            Invoke(nameof(ResetCanShakeCamera), 0.1f);
            canShackCamera = false;
            impulseSource.GenerateImpulseWithForce(shakeForce);
        }
    }

    private void ResetCanShakeCamera() => canShackCamera = true;

}
