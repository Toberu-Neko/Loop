using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.CheckIfShouldFlip(xInput);

        PlayStepSFX(0.4f);

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


}
