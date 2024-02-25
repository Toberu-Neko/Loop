using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //TODO: Should not enter if wall degree is greater than 95

        player.JumpState.ResetAmountOfJumpsLeft();
        Movement.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        Movement.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
        AudioManager.instance.PlaySoundFX(player.PlayerSFX.jump, Movement.ParentTransform, AudioManager.SoundType.twoD);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        if(Time.time >= StartTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }
    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -Movement.FacingDirection;
        }
        else
        {
            wallJumpDirection = Movement.FacingDirection;
        }
    }
}
