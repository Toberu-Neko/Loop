using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
    private SetCollider setCollider;
    public PlayerCrouchMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        setCollider = player.Core.GetCoreComponent<SetCollider>();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetColliderHeight(playerData.crouchColliderHeight);
        setCollider.isCrouching = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetColliderHeight(playerData.standColliderHeight);
        setCollider.isCrouching = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.CheckIfShouldFlip(xInput);

        if (!isExitingState)
        {
            Movement.SetVelocityX(playerData.movementVelocity * playerData.crouchMovementMultiplier * xInput);

            if (xInput == 0)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else if (yInput != -1 && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
