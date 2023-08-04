using Cinemachine;
using System;
using UnityEditor;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CamControlObjects camControlObjects;

    [SerializeField] private Collider2D col;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (camControlObjects.panCamOnContact)
            {
                CamManager.instance.PanCameraOnTrigger(camControlObjects.panDistance, camControlObjects.panTime, camControlObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - col.bounds.center).normalized;

            if (camControlObjects.swapCam && camControlObjects.cameraOnLeft != null && camControlObjects.cameraOnRight != null)
            {
                CamManager.instance.SwapCamera(camControlObjects.cameraOnLeft, camControlObjects.cameraOnRight, exitDirection);
            }
            if(camControlObjects.panCamOnContact)
            {
                CamManager.instance.PanCameraOnTrigger(camControlObjects.panDistance, camControlObjects.panTime, camControlObjects.panDirection, true);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (!TryGetComponent<BoxCollider2D>(out var boxCollider))
            return;

        Gizmos.color = Color.white;

        Bounds bounds = boxCollider.bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}

[Serializable]
public class CamControlObjects
{
    public bool swapCam = false;
    public bool panCamOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 1f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraControlTrigger))]
public class CamControlEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (cameraControlTrigger.camControlObjects.swapCam)
        {
            cameraControlTrigger.camControlObjects.cameraOnLeft = 
                (CinemachineVirtualCamera)EditorGUILayout.ObjectField("Camera On Left", cameraControlTrigger.camControlObjects.cameraOnLeft, typeof(CinemachineVirtualCamera), true);

            cameraControlTrigger.camControlObjects.cameraOnRight = 
                (CinemachineVirtualCamera)EditorGUILayout.ObjectField("Camera On Right", cameraControlTrigger.camControlObjects.cameraOnRight, typeof(CinemachineVirtualCamera), true);
        }

        if (cameraControlTrigger.camControlObjects.panCamOnContact)
        {
            cameraControlTrigger.camControlObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Pan Direction", cameraControlTrigger.camControlObjects.panDirection);
            cameraControlTrigger.camControlObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.camControlObjects.panDistance);
            cameraControlTrigger.camControlObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.camControlObjects.panTime);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(cameraControlTrigger);
    }
}
#endif
