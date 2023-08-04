using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected bool jumpInput;
    protected int xInput;
    protected int yInput;

    protected bool isTouchingLedge;

    private bool damaged;
    private float endTime;

    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        if(isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Enter()
    {
        base.Enter();

        damaged = false;
        Combat.OnDamaged += HandleOnDamaged;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= HandleOnDamaged;
        endTime = Time.time;
    }

    private void HandleOnDamaged()
    {
        damaged = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        Movement.CheckIfShouldFlip(xInput);
        Movement.SetVelocityX(0f);

        if (damaged)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if (jumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall || (xInput != Movement.FacingDirection && !grabInput))
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isTouchingWall && !isTouchingLedge && !Stats.IsRewindingPosition)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }

    public virtual bool CheckCanClimbWall()
    {
        return Time.time >= endTime + 1f;
    }
}
