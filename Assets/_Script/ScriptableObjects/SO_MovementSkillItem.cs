using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSkillItem", menuName = "Items/MovementSkillItem")]
public class SO_MovementSkillItem : SO_ItemsBase
{
    [Header("�̤U�����ӥ[�����n��, ���F�|�a")]

    public List<UnlockSkillFields> unlockSkill = new()
    {
        {new( "DoubleJump", false ) },
        {new( "Dash", false ) },
        {new("WallJump", false ) }
    };
}
