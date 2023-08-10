using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSceneState : PlayerState
{
    private int facingDirection;
    private bool canChangeState;
    public PlayerChangeSceneState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Movement.CheckIfShouldFlip(facingDirection);
        canChangeState = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityX(playerData.movementVelocity * facingDirection);

        if(canChangeState && CollisionSenses.Ground)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    public void SetFacingDirection(int direction)
    {
        facingDirection = direction;
    }

    public void SetCanChangeStateTrue()
    {
        canChangeState = true;
    }
}
