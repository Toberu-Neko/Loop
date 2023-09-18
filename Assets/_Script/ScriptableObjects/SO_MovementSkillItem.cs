using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSkillItem", menuName = "Items/MovementSkillItem")]
public class SO_MovementSkillItem : SO_ItemsBase
{
    [Header("最下面那個加號不要按, 按了會壞")]

    public List<UnlockSkillFields> unlockSkill = new()
    {
        {new( "DoubleJump", false ) },
        {new( "Dash", false ) },
        {new("WallJump", false ) }
    };
}
