using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeSkillItem", menuName = "Items/TimeSkillItem")]
public class SO_TimeSkillItem : SO_ItemsBase
{
    [Header("最下面那個加號不要按, 按了會壞")]

    public List<UnlockSkillFields> unlockSkill = new()
    {
        {new("TimeStopRanged", false) },
        {new("TimeStopAll", false) },
        {new("TimeSlowRanged", false) },
        {new("TimeSlowAll", false) },
        {new("TimeReverse", false) },
        {new("BookMark", false) }
    };

}

[System.Serializable]
public class UnlockSkillFields
{
    [HideInInspector] public string name;
    public bool unlock;

    public UnlockSkillFields(string name, bool value)
    {
        this.name = name;
        this.unlock = value;
    }
}
