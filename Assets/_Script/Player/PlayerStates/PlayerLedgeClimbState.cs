using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPosition;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;

    private Vector2 v2Workspace;

    private bool isHanging;
    private bool isClimbing;
    private bool isTouchingCeiling;

    private bool jumpInput;

    private int xInput;
    private int yInput;

    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = DeterminCornerPosition();

        startPosition.Set(cornerPosition.x - (Movement.FacingDirection * playerData.startOffset.x), cornerPosition.y - playerData.startOffset.y);
        stopPosition.Set(cornerPosition.x + (Movement.FacingDirection * playerData.stopOffset.x), cornerPosition.y + playerData.stopOffset.y);

        player.transform.position = startPosition;
        Combat.OnDamaged += HandleOnDamaged;
    }

    public override void Exit()
    {
        base.Exit();
        isHanging = false;

        if(isClimbing)
        {
            player.transform.position = stopPosition;
            isClimbing = false;
        }

        Combat.OnDamaged -= HandleOnDamaged;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if(isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            Movement.SetVelocityZero();
            player.transform.position = startPosition;

            if (Stats.IsRewindingPosition)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (xInput == Movement.FacingDirection && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if(yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    private void HandleOnDamaged() => stateMachine.ChangeState(player.InAirState);

    public void SetDetectedPosition(Vector2 pos) => detectedPosition = pos;

    private void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPosition + (Vector2.up * 0.015f) + (0.015f * Movement.FacingDirection * Vector2.right), Vector2.up, playerData.standColliderHeight, CollisionSenses.WhatIsGround);
        player.Anim.SetBool("isTouchingCeiling", isTouchingCeiling);
    }
    private Vector2 DeterminCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(CollisionSenses.WallCheck.position, Vector2.right * Movement.FacingDirection, CollisionSenses.WallCheckDistance, CollisionSenses.WhatIsGround);
        float xDist = xHit.distance;
        v2Workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);

        RaycastHit2D yHit = Physics2D.Raycast(CollisionSenses.LedgeCheckHorizontal.position + (Vector3)v2Workspace, Vector2.down, CollisionSenses.LedgeCheckHorizontal.position.y - CollisionSenses.WallCheck.position.y + 0.015f, CollisionSenses.WhatIsGround);
        float yDist = yHit.distance;
        v2Workspace.Set(CollisionSenses.WallCheck.position.x + (xDist * Movement.FacingDirection), CollisionSenses.WallCheck.position.y - yDist);

        return v2Workspace;
    }
}
