using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunNormalAttackState : PlayerAbilityState
{
    SO_WeaponData_Gun data;
    public PlayerGunNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        // GameObject.Instantiate(data.normalAttackObject);
    }
}
