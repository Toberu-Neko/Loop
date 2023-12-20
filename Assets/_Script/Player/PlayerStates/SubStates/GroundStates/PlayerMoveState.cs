using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private float lastStepTime;
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        lastStepTime = 0f;
    }

    public override void Enter()
    {
        base.Enter();

        PlayStepSFX();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.CheckIfShouldFlip(xInput);

        PlayStepSFX();

        if (!isExitingState)
        {
            if (CollisionSenses.UnclimbableWallFront)
            {
                Movement.SetVelocityX(0f);
            }
            else
            {
                Movement.SetVelocityX(playerData.movementVelocity * xInput);
            }

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(yInput == -1) 
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }
    }

    private void PlayStepSFX()
    {
        if (Time.time>lastStepTime + 0.4f)
        {
            lastStepTime = Time.time;
            AudioManager.instance.PlaySoundFX(player.PlayerSFX.footstep, player.transform, 1f);
        }
    }

}
