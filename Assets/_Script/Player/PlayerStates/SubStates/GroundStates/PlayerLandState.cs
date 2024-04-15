using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerAbilityState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySoundFX(player.PlayerSFX.land, Movement.ParentTransform, AudioManager.SoundType.twoD);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(!isExitingState)
        {
            Movement.SetVelocityZero();
            if (player.NoHand)
            {
                player.Anim.speed = 1f;
            }
            else if (player.InputHandler.NormInputX != 0)
            {
                player.Anim.speed = playerData.speedUpLandAnimMulitiplier;
            }
            else
            {
                player.Anim.speed = 1f;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.Anim.speed = 1f;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (!isExitingState) 
            stateMachine.ChangeState(player.IdleState);
    }
}
