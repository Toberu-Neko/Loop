using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSceneState : PlayerState
{
    private int facingDirection;
    public PlayerChangeSceneState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Movement.CheckIfShouldFlip(facingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityX(playerData.movementVelocity * facingDirection);
    }

    public void SetFacingDirection(int direction)
    {
        facingDirection = direction;
    }
}
