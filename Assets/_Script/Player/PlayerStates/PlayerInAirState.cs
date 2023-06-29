using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isJumping;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool isToucingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;

    protected Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;

    protected Stats Stats => stats ? stats : core.GetCoreComponent<Stats>();
    private Stats stats;
    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isToucingWallBack;

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isToucingWallBack = CollisionSenses.WallBack;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
        }

        if(isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
        if(!wallJumpCoyoteTime && !isTouchingWall && !isToucingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
        else if (isTouchingWall || isToucingWallBack)
        {
            StopWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack= false;
        isTouchingWall = false;
        isToucingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;

        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        #region Sword
        if (player.InputHandler.AttackInput && player.PlayerWeaponManager.CurrentWeaponType == PlayerWeaponType.Sword &&
                    player.SwordHubState.CheckIfCanAttack())
        {
            stateMachine.ChangeState(player.SwordHubState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.PlayerWeaponManager.CurrentWeaponType == PlayerWeaponType.Sword
            && player.SwordHubState.CheckIfCanAttack() &&
            player.PlayerWeaponManager.SwordCurrentEnergy > 0 &&
            Stats.PerfectBlockAttackable &&
            player.PlayerWeaponManager.SwordCurrentEnergy < player.PlayerWeaponManager.SwordData.maxEnergy)
        {
            player.SwordHubState.SetCanAttackFalse();
            stateMachine.ChangeState(player.PlayerSwordSoulOneAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.PlayerWeaponManager.CurrentWeaponType == PlayerWeaponType.Sword &&
            player.SwordHubState.CheckIfCanAttack() && player.PlayerWeaponManager.SwordCurrentEnergy > 0
            && player.PlayerWeaponManager.SwordCurrentEnergy == player.PlayerWeaponManager.SwordData.maxEnergy)
        {
            player.SwordHubState.SetCanAttackFalse();
            stateMachine.ChangeState(player.PlayerSwordSoulMaxAttackState);
        }
        #endregion
        else if (player.InputHandler.AttackInput && player.PlayerWeaponManager.CurrentWeaponType == PlayerWeaponType.Gun)
        {
            stateMachine.ChangeState(player.PlayerGunNormalAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.PlayerWeaponManager.CurrentWeaponType == PlayerWeaponType.Gun)
        {
            stateMachine.ChangeState(player.PlayerGunChargeAttackState);
        }
        else if (player.InputHandler.BlockInput && player.BlockState.CheckIfCanBlock())
        {
            stateMachine.ChangeState(player.BlockState);
        }
        else if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isToucingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionSenses.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if(isTouchingWall && xInput == Movement.FacingDirection && Movement.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            Movement.CheckIfShouldFlip(xInput);
            Movement.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        }
    }
    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time >= startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }
    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime()
    {
        coyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StartWallJumpCoyoteTime() => wallJumpCoyoteTime = true;
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void SetIsJumping() => isJumping = true;
}
