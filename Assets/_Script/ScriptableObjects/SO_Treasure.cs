using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Treasure", menuName = "Data/Treasure")]
public class SO_Treasure : ScriptableObject
{
    public TreasureType treasureType;

    public string treasureName = "Defult Item";
    public string treasureDescription = "Defult Description";

    [Header("Chip")]
    public SO_Chip chip;

    [Header("StoryItem")]
    public string storyItemName = "Defult Item";
    public string storyItemDescription = "Defult Description";

    [Header("PlayerStatusEnhancement")]
    public SO_PlayerStatusEnhancement playerStatusEnhancement;

    [Header("Skill")]
    public PlayerTimeSkills timeSkills;

    [Header("Movement")]
    public PlayerMovementSkills playerMovementSkills;

    public enum TreasureType
    {
        Chip,
        PlayerStatusEnhancement,
        StoryItem,
        Movement,
        TimeSkill
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SO_Treasure))]
public class TreasureEditor : Editor
{
    SO_Treasure so;

    private void OnEnable()
    {
        so = (SO_Treasure)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();



        if (GUI.changed)
            EditorUtility.SetDirty(so);
        /*

        if (cameraControlTrigger.camControlObjects.swapTriggerOption == CamControlObjects.SwapTriggerOption.swap)
        {
            cameraControlTrigger.camControlObjects.cameraOnLeft =
                (CinemachineVirtualCamera)EditorGUILayout.ObjectField("Camera On Left", cameraControlTrigger.camControlObjects.cameraOnLeft, typeof(CinemachineVirtualCamera), true);

            cameraControlTrigger.camControlObjects.cameraOnRight =
                (CinemachineVirtualCamera)EditorGUILayout.ObjectField("Camera On Right", cameraControlTrigger.camControlObjects.cameraOnRight, typeof(CinemachineVirtualCamera), true);
        }

        if (cameraControlTrigger.camControlObjects.swapTriggerOption == CamControlObjects.SwapTriggerOption.pan)
        {
            cameraControlTrigger.camControlObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Pan Direction", cameraControlTrigger.camControlObjects.panDirection);
            cameraControlTrigger.camControlObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.camControlObjects.panDistance);
            cameraControlTrigger.camControlObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.camControlObjects.panTime);
        }


        */
    }
}
#endif