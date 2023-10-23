using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    private SetCollider setCollider;
    public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        setCollider = player.Core.GetCoreComponent<SetCollider>();
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
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

        if(!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (yInput != -1 && !isTouchingCeiling)
            {
                // Debug.Log(isTouchingCeiling);
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
