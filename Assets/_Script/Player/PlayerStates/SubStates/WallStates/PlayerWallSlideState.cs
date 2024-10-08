using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            Movement.SetVelocityY(-playerData.wallSlideVelocity);

            if(grabInput && yInput == 0 && player.PlayerData.canWallClimb)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if(!isTouchingWall || (xInput != Movement.FacingDirection && !grabInput))
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }
}
