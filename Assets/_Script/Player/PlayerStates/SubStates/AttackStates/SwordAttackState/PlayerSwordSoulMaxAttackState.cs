using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSoulMaxAttackState : PlayerSwordAttackState
{
    WeaponAttackDetails details;
    public PlayerSwordSoulMaxAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.PlayerWeaponManager.SwordData.soulThreeAttackDetails;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Sword, details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce, false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.PlayerWeaponManager.ClearCurrentEnergy();
    }

}
