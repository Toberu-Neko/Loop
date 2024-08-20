using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected bool isTouchingCeiling;

    private bool jumpInput;
    private bool grabInput;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    protected bool isOnSlope;

    private PlayerWeaponManager weaponManager;

    private float lastStepTime;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        weaponManager = player.WeaponManager;
        lastStepTime = 0f;
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

        #region Input Checks
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        #endregion

        #region Sword
        if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == WeaponType.Sword && !isTouchingCeiling &&
            player.SwordHubState.CheckIfCanAttack())
        {
            stateMachine.ChangeState(player.SwordHubState);
        }
        else if (player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == WeaponType.Sword && !isTouchingCeiling
            && player.SwordHubState.CheckIfCanAttack() && weaponManager.SwordCurrentEnergy > 0 &&
            Stats.Attackable &&
            weaponManager.SwordCurrentEnergy < weaponManager.SwordData.maxEnergy && !player.WeaponManager.EnhanceSwordAttack)
        {
            stateMachine.ChangeState(player.SwordEnhanceState);
        }
        else if (player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == WeaponType.Sword && !isTouchingCeiling
            && player.SwordHubState.CheckIfCanAttack() && weaponManager.SwordCurrentEnergy > 0 &&
            Stats.Attackable &&
            weaponManager.SwordCurrentEnergy == weaponManager.SwordData.maxEnergy)
        {
            stateMachine.ChangeState(player.SwordSoulMaxAttackState);
        }
        #endregion

        #region Gun
        else if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == WeaponType.Gun && !isTouchingCeiling && Stats.Attackable
            && player.GunNormalAttackState.CheckCanAttack() && !Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.GunHubState);
        }
        else if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == WeaponType.Gun && !isTouchingCeiling && Stats.Attackable && Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.GunCounterAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == WeaponType.Gun && 
            weaponManager.GunCurrentEnergy > 0 && weaponManager.GunCurrentEnergy < weaponManager.GunData.maxGrenade && Stats.Attackable)
        {
            player.InputHandler.UseWeaponSkillInput();
            stateMachine.ChangeState(player.GunThrowGrenadeState);
        }
        else if (player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == WeaponType.Gun && weaponManager.GunCurrentEnergy == weaponManager.GunData.maxGrenade && Stats.Attackable)
        {
            player.InputHandler.UseWeaponSkillInput();
            stateMachine.ChangeState(player.GunS3State);

        }
        #endregion

        #region Fist
        else if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == WeaponType.Fist && !isTouchingCeiling && Stats.Attackable && !Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.FistHubState);
        }
        else if (player.InputHandler.AttackInput && weaponManager.CurrentWeaponType == WeaponType.Fist && !isTouchingCeiling && Stats.Attackable && Stats.CounterAttackable)
        {
            stateMachine.ChangeState(player.FistCounterAttackState);
        }
        else if (player.InputHandler.WeaponSkillInput && weaponManager.CurrentWeaponType == WeaponType.Fist && !isTouchingCeiling && Stats.Attackable &&
            weaponManager.FistCurrentEnergy == weaponManager.FistData.maxEnergy)
        {
            stateMachine.ChangeState(player.FistS3ChargeState);
        }
        #endregion

        #region Movement
        else if (player.InputHandler.RegenInput && isGrounded && Stats.Health.CurrentValue < Stats.Health.MaxValue &&
            (PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].itemCount > 0)
            )
        {
            player.InputHandler.UseRegenInput();
            stateMachine.ChangeState(player.RegenState);
        }
        else if (jumpInput && yInput < 0 && CollisionSenses.GroundPlatform)
        {
            if (!CollisionSenses.GroundPlatform.collider.CompareTag("Elevator"))
            {
                Physics2D.IgnoreCollision(player.MovementCollider, CollisionSenses.GroundPlatform.collider, true);
            }
        }

        else if (player.InputHandler.BlockInput && !isTouchingCeiling && player.PreBlockState.CheckIfCanBlock() )
        {
            stateMachine.ChangeState(player.PreBlockState);
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
        else if (isTouchingWall && grabInput && isTouchingLedge && player.PlayerData.canWallClimb)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(player.DashState);
        }
        #endregion
    }

    /// <summary>
    /// Play footstep sound effect, time is the interval between each step sound effect
    /// </summary>
    /// <param name="time">The interval between each step sound effect</param>
    protected void PlayStepSFX(float time)
    {
        if (Time.time > lastStepTime + time)
        {
            lastStepTime = Time.time;
            AudioManager.Instance.PlaySoundFX(player.PlayerSFX.footstep, player.transform, AudioManager.SoundType.twoD);
        }
    }

}
