using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input
    private int xInput;
    private int yInput;
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

            if (CollisionSenses.HeadPlatform)
            {
                Physics2D.IgnoreCollision(player.MovementCollider, CollisionSenses.HeadPlatform.collider, true);
            }

            if (CollisionSenses.GroundPlatform)
            {
                Physics2D.IgnoreCollision(player.MovementCollider, CollisionSenses.GroundPlatform.collider, false);
            }
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
        isJumping = false;
        jumpInputStop = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();


        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;


        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        #region Sword
        if (player.InputHandler.AttackInput && player.WeaponManager.CurrentWeaponType == WeaponType.Sword &&
                    player.SwordHubState.CheckIfCanAttack() && Stats.Attackable)
        {
            stateMachine.ChangeState(player.SwordHubState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.WeaponManager.CurrentWeaponType == WeaponType.Sword &&
            player.SwordHubState.CheckIfCanAttack() && player.WeaponManager.SwordCurrentEnergy > 0
            && player.WeaponManager.SwordCurrentEnergy == player.WeaponManager.SwordData.maxEnergy && Stats.Attackable)
        {
            player.SwordHubState.SetCanAttackFalse();
            stateMachine.ChangeState(player.SwordSoulMaxAttackState);
        }
        #endregion

        #region Gun
        else if (player.InputHandler.AttackInput && player.WeaponManager.CurrentWeaponType == WeaponType.Gun && Stats.Attackable
            && player.GunNormalAttackState.CheckCanAttack() && !Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.GunHubState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.WeaponManager.CurrentWeaponType == WeaponType.Gun && Stats.Attackable && Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.GunCounterAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.WeaponManager.CurrentWeaponType == WeaponType.Gun && Stats.Attackable && player.WeaponManager.GrenadeCount > 0 && player.WeaponManager.GrenadeCount < player.WeaponManager.GunData.maxGrenade)
        {
            player.InputHandler.UseWeaponSkillInput();
            stateMachine.ChangeState(player.GunThrowGrenadeState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.WeaponManager.CurrentWeaponType == WeaponType.Gun && player.WeaponManager.GrenadeCount == player.WeaponManager.GunData.maxGrenade && Stats.Attackable)
        {
            player.InputHandler.UseWeaponSkillInput();
            stateMachine.ChangeState(player.GunS3State);

        }
        #endregion

        #region Fist
        else if (player.InputHandler.AttackInput && player.WeaponManager.CurrentWeaponType == WeaponType.Fist && Stats.Attackable && !Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.FistHubState);
        }
        else if (player.InputHandler.AttackInput && player.WeaponManager.CurrentWeaponType == WeaponType.Fist && Stats.Attackable && Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.FistCounterAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && player.WeaponManager.CurrentWeaponType == WeaponType.Fist && Stats.Attackable &&
            player.WeaponManager.FistCurrentEnergy == player.WeaponManager.FistData.maxEnergy)
        {
            stateMachine.ChangeState(player.FistS3ChargeState);
        }
        #endregion

        else if (player.InputHandler.BlockInput && player.BlockState.CheckIfCanBlock() && Stats.Attackable)
        {
            stateMachine.ChangeState(player.BlockState);
        }
        else if (isGrounded && !isJumping)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (yInput >= 0 && player.LedgeClimbState.CheckCanGrabWall() && isTouchingWall && !isTouchingLedge && !isGrounded && !Stats.IsRewindingPosition)
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
        else if(jumpInput && yInput >= 0 && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge && player.WallGrabState.CheckCanClimbWall())
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if(isTouchingWall && xInput == Movement.FacingDirection && Movement.CurrentVelocity.y <= 0 && player.WallSlideState.CheckCanClimbWall())
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
            Movement.SetVelocityX(playerData.movementVelocity * xInput, true);

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
                Movement.SetVelocityY(Movement.CurrentVelocity.y * playerData.jumpInpusStopYSpeedMultiplier);

                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y < -0.01f)
            {
                isJumping = false;
            }
        }
    }
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time >= StartTime + playerData.coyoteTime)
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
    public void SetIsJumping()
    {
        isJumping = true;
    }
}
