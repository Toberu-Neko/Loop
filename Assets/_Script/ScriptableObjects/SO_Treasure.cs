using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Treasure", menuName = "Data/Treasure")]
public class SO_Treasure : ScriptableObject
{
    public TreasureType treasureType;

    [Header("Chip")]
    public SO_Chip chip;

    [Header("StoryItem")]
    public SO_StoryItem storyItem;

    [Header("PlayerStatusEnhancement")]
    public SO_PlayerStatusEnhancement playerStatusEnhancement;

    [Header("Skill")]
    public SO_TimeSkillItem timeSkills;

    [Header("Movement")]
    public SO_MovementSkillItem movementSkills;

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

        /*
        so.treasureType = (SO_Treasure.TreasureType) EditorGUILayout.EnumPopup("Treasure Type", so.treasureType);

        if(so.treasureType == SO_Treasure.TreasureType.Chip)
        {
            EditorGUILayout.LabelField("Chip", EditorStyles.boldLabel);
            so.chip = (SO_Chip)EditorGUILayout.ObjectField("Chip", so.chip, typeof(SO_Chip), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.StoryItem)
        {
            EditorGUILayout.LabelField("Story Item", EditorStyles.boldLabel);
            so.storyItem = (SO_StoryItem)EditorGUILayout.ObjectField("Story Item", so.storyItem, typeof(SO_StoryItem), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.PlayerStatusEnhancement)
        {
            EditorGUILayout.LabelField("Player Status Enhancement", EditorStyles.boldLabel);
            so.playerStatusEnhancement = (SO_PlayerStatusEnhancement)EditorGUILayout.ObjectField("Player Status Enhancement", so.playerStatusEnhancement, typeof(SO_PlayerStatusEnhancement), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.TimeSkill)
        {
            EditorGUILayout.LabelField("Time Skill", EditorStyles.boldLabel);
            so.timeSkills = (SO_TimeSkillItem)EditorGUILayout.ObjectField("Time Skill", so.timeSkills, typeof(SO_TimeSkillItem), true);
        }

        if (so.treasureType == SO_Treasure.TreasureType.Movement)
        {
            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
            so.movementSkills = (SO_MovementSkillItem)EditorGUILayout.ObjectField("Movement", so.movementSkills, typeof(SO_MovementSkillItem), true);
        }
        */

        if (GUI.changed)
            EditorUtility.SetDirty(so);
    }
}
#endif