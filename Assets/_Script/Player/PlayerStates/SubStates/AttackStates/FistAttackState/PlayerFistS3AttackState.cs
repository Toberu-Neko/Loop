using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistS3AttackState : PlayerFistAttackState
{
    SO_WeaponData_Fist data;
    private WeaponAttackDetails[] details;
    public PlayerFistS3AttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
        details = data.soulThreeAttackDetails;
    }

    
}
