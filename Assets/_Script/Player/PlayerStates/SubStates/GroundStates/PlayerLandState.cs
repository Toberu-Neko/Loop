using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySoundFX(player.PlayerSFX.land, Movement.ParentTransform);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(!isExitingState)
        {
            Movement.SetVelocityZero();
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (!isExitingState) 
            stateMachine.ChangeState(player.IdleState);
    }
}
