using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private GameManager gameManager;

    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public OldPlayerAttackState AttackState { get; private set; }
    public PlayerBlockState BlockState { get; private set; }
    public PlayerPerfectBlockState PerfectBlockState { get; private set; }
    #endregion

    #region SwordStates
    public PlayerSwordHubState SwordHubState { get; private set; }
    public PlayerSwordNormalAttackState SwordNormalAttackState { get; private set; }
    public PlayerSwordStrongAttackState SwordStrongAttackState { get; private set; }
    public PlayerSwordSkyAttackState SwordSkyAttackState { get; private set; }
    public PlayerSwordSoulOneAttackState SwordCounterAttackState { get; private set; }
    public PlayerSwordSoulOneAttackState SwordSoulOneAttackState { get; private set; }
    public PlayerSwordSoulMaxAttackState SwordSoulMaxAttackState { get; private set; }
    #endregion

    #region GunStates
    public PlayerGunNormalAttackState GunNormalAttackState { get; private set; }
    public PlayerGunChargingState GunChargeAttackState { get; private set; }
    #endregion

    #region FistStates
    public PlayerFistHubState FistHubState { get; private set; }
    public PlayerFistNormalAttackState FistNormalAttackState { get; private set; }
    public PlayerFistSoulAttackState FistSoulAttackState { get; private set; }
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerWeaponManager PlayerWeaponManager { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    #endregion

    #region Other Variables
    // public int FacingDirection { get; private set; }

    private Vector2 v2Workspace;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        MovementCollider = GetComponent<BoxCollider2D>();
        PlayerWeaponManager = GetComponent<PlayerWeaponManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        AttackState = new OldPlayerAttackState(this, StateMachine, playerData, "attack");
        BlockState = new PlayerBlockState(this, StateMachine, playerData, "block");
        PerfectBlockState = new PlayerPerfectBlockState(this, StateMachine, playerData, "perfectBlock");

        SwordHubState = new PlayerSwordHubState(this, StateMachine, playerData, "swordAttack");
        SwordNormalAttackState = new PlayerSwordNormalAttackState(this, StateMachine, playerData, "swordNormalAttack");
        SwordStrongAttackState = new PlayerSwordStrongAttackState(this, StateMachine, playerData, "swordStrongAttack");
        SwordSkyAttackState = new PlayerSwordSkyAttackState(this, StateMachine, playerData, "swordSkyAttack");
        SwordCounterAttackState = new PlayerSwordSoulOneAttackState(this, StateMachine, playerData, "swordCounterAttack");
        SwordSoulOneAttackState = new PlayerSwordSoulOneAttackState(this, StateMachine, playerData, "swordSoulOneAttack");
        SwordSoulMaxAttackState = new PlayerSwordSoulMaxAttackState(this, StateMachine, playerData, "swordSoulMaxAttack");

        GunNormalAttackState = new PlayerGunNormalAttackState(this, StateMachine, playerData, "gunNormalAttack");
        GunChargeAttackState = new PlayerGunChargingState(this, StateMachine, playerData, "gunChargeAttack");

        FistHubState = new PlayerFistHubState(this, StateMachine, playerData, "fistAttack");
        FistNormalAttackState = new PlayerFistNormalAttackState(this, StateMachine, playerData, "fistNormalAttack");
        FistSoulAttackState = new PlayerFistSoulAttackState(this, StateMachine, playerData, "fistSoulAttack");
    }

    private void Start()
    {
        // TODO: SetWeapon
        // AttackState.SetWeapon(Inventory.weapons[0]);

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();

        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Other Functions
    public void SetColliderHeight(float height)
    {
        //TODO: Delete this, use animation instead
        Vector2 center = MovementCollider.offset;
        v2Workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = v2Workspace;
        MovementCollider.offset = center;

    }

    private void AnimationActionTrigger() => StateMachine.CurrentState.AnimationActionTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void AnimationStartMovementTrigger() => StateMachine.CurrentState.AnimationStartMovementTrigger();

    private void AnimationStopMovementTrigger() => StateMachine.CurrentState.AnimationStopMovementTrigger();


    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerData.perfectBlockKnockbackRadius);
    }
}
