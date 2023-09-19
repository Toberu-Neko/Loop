using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeSkillItem", menuName = "Items/TimeSkillItem")]
public class SO_TimeSkillItem : SO_ItemsBase
{
    public PlayerTimeSkills unlockedSkills;

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
