using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSoulOneAttackState : PlayerAbilityState
{
    public PlayerSwordSoulOneAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetPerfectBlockAttackFalse();
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
    }
}
