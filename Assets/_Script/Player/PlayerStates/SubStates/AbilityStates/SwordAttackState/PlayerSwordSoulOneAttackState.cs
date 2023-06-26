using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSoulOneAttackState : PlayerAbilityState
{
    WeaponAttackDetails details;
    public PlayerSwordSoulOneAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.PlayerWeaponManager.SwordData.soulOneAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetPerfectBlockAttackFalse();
        player.PlayerWeaponManager.DecreaseEnergy();
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(details.damageAmount,details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
    }
}
