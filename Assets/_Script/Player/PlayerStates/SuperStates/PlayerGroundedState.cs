using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    protected Stats Stats => stats ? stats : core.GetCoreComponent<Stats>();
    private Stats stats;

    protected Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;
    private CollisionSenses CollisionSenses 
    { 
        // ?? == if left is null, return right, else return left
        get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();


        /* get
        {
            if(collisionSenses)
            {
                return collisionSenses;
            }
            collisionSenses = core.GetCoreComponent<CollisionSenses>();
            return collisionSenses;
        }*/ 
    }
    private CollisionSenses collisionSenses;

    private bool jumpInput;
    private bool grabInput;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            isTouchingCeiling = CollisionSenses.Ceiling;
        }
        // Debug.Log("PGS: " + isTouchingCeiling);
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();
        player.SwordHubState.ResetCanAttack();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        // Debug.Log(xInput);

        if (player.InputHandler.AttackInput && !isTouchingCeiling && player.SwordHubState.CheckIfCanAttack())
        {
            stateMachine.ChangeState(player.SwordHubState);
        }
        else if(player.InputHandler.WeaponSkillInput && !isTouchingCeiling && player.SwordHubState.CheckIfCanAttack() && 
            player.PlayerWeaponManager.SwordCurrentEnergy > 0 &&
            Stats.PerfectBlockAttackable && 
            player.PlayerWeaponManager.SwordCurrentEnergy < player.PlayerWeaponManager.SwordData.maxEnergy)
        {
            player.SwordHubState.SetCanAttackFalse();
            stateMachine.ChangeState(player.PlayerSwordSoulOneAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && !isTouchingCeiling &&
            player.SwordHubState.CheckIfCanAttack() && player.PlayerWeaponManager.SwordCurrentEnergy > 0
            && player.PlayerWeaponManager.SwordCurrentEnergy == player.PlayerWeaponManager.SwordData.maxEnergy)
        {
            player.SwordHubState.SetCanAttackFalse();
            Debug.Log("PlayerSwordSoulMaxAttackState");
            // TODO: stateMachine.ChangeState(player.PlayerSwordSoulMaxAttackState);
        }
        else if (player.InputHandler.BlockInput && !isTouchingCeiling && player.BlockState.CheckIfCanBlock())
        {
            stateMachine.ChangeState(player.BlockState);
        }
        else if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isTouchingWall && grabInput && isTouchingLedge)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
