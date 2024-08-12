using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.CheckIfShouldFlip(xInput);

        #region SFX
        if (player.NoHand)
        {
            PlayStepSFX(0.8f);
        }
        else
        {
            PlayStepSFX(0.4f);
        }
        #endregion

        if (!isExitingState)
        {
            if (CollisionSenses.UnclimbableWallFront)
            {
                Movement.SetVelocityX(0f);
            }
            else if (player.NoHand)
            {
                Movement.SetVelocityX(playerData.noHandMovementVelocity * xInput);
            }
            else
            {
                Movement.SetVelocityX(playerData.movementVelocity * xInput);
            }

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if(yInput == -1 && !player.NoHand) 
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
        }
    }
}
