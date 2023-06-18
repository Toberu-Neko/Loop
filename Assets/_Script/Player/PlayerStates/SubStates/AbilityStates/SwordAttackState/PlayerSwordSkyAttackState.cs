using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkyAttackState : PlayerAbilityState
{
    public PlayerSwordSkyAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        Debug.Log("Do Sword Sky Attack");
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

}
