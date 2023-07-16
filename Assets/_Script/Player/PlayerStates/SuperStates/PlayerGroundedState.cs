using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;


    protected CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    private bool jumpInput;
    private bool grabInput;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    protected bool isOnSlope;

    private PlayerWeaponManager weaponManager;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        weaponManager = player.PlayerWeaponManager;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingCeiling = CollisionSenses.SolidCeiling;
        }
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

        #region Sword
        if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == PlayerWeaponType.Sword && !isTouchingCeiling && 
            player.SwordHubState.CheckIfCanAttack())
        {
            stateMachine.ChangeState(player.SwordHubState);
        }
        else if(player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == PlayerWeaponType.Sword && !isTouchingCeiling 
            && player.SwordHubState.CheckIfCanAttack() && 
            weaponManager.SwordCurrentEnergy > 0 &&
            Stats.PerfectBlockAttackable && 
            weaponManager.SwordCurrentEnergy < weaponManager.SwordData.maxEnergy)
        {
            player.SwordHubState.SetCanAttackFalse();
            stateMachine.ChangeState(player.SwordSoulOneAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == PlayerWeaponType.Sword && !isTouchingCeiling &&
            player.SwordHubState.CheckIfCanAttack() && weaponManager.SwordCurrentEnergy > 0
            && weaponManager.SwordCurrentEnergy == weaponManager.SwordData.maxEnergy)
        {
            player.SwordHubState.SetCanAttackFalse();
            stateMachine.ChangeState(player.SwordSoulMaxAttackState);
        }
        #endregion

        #region Gun
        else if(player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == PlayerWeaponType.Gun && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.GunNormalAttackState);
        }
        else if(player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == PlayerWeaponType.Gun && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.GunChargeAttackState);
        }
        #endregion

        #region Fist
        else if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == PlayerWeaponType.Fist && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.FistHubState);
        }
        else if(player.InputHandler.WeaponSkillInput && yInput >= 0 && weaponManager.CurrentWeaponType == PlayerWeaponType.Fist && !isTouchingCeiling && weaponManager.FistCurrentEnergy == weaponManager.FistData.maxEnergy)
        {
            player.FistSoulAttackState.SetStaticAttack(false);
            weaponManager.ClearCurrentEnergy();
            player.FistSoulAttackState.SetSoulAmount(weaponManager.FistData.maxEnergy - 1);
            stateMachine.ChangeState(player.FistSoulAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && yInput < 0 && weaponManager.CurrentWeaponType == PlayerWeaponType.Fist && !isTouchingCeiling && weaponManager.FistCurrentEnergy == weaponManager.FistData.maxEnergy)
        {
            player.FistSoulAttackState.SetStaticAttack(true);
            weaponManager.ClearCurrentEnergy();
            player.FistSoulAttackState.SetSoulAmount(weaponManager.FistData.maxEnergy - 1);
            stateMachine.ChangeState(player.FistSoulAttackState);
        }
        #endregion

        else if (player.InputHandler.BlockInput && !isTouchingCeiling && player.BlockState.CheckIfCanBlock())
        {
            stateMachine.ChangeState(player.BlockState);
        }
        else if(jumpInput && yInput < 0 && CollisionSenses.GroundPlatform)
        {
            Physics2D.IgnoreCollision(player.MovementCollider, CollisionSenses.GroundPlatform.collider, true);
        }
        else if (jumpInput && yInput >= 0 && player.JumpState.CanJump() && !isTouchingCeiling)
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
}
