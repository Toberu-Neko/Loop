using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordCounterAttackState : PlayerAbilityState
{
    WeaponAttackDetails details;
    public PlayerSwordCounterAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.SwordData.strongAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetPerfectBlockAttackFalse();
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(details.damageAmount, details.knockbackAngle, details.knockbackForce);
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
