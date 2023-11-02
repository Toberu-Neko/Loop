using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerData PlayerData { get;private set; }
    private GameManager gameManager;

    #region ControlerStates
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerChangeSceneState ChangeSceneState { get; private set; }
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
    public PlayerBlockState BlockState { get; private set; }
    public PlayerPerfectBlockState PerfectBlockState { get; private set; }
    public PlayerRegenState RegenState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    #endregion

    #region SwordStates
    public PlayerSwordHubState SwordHubState { get; private set; }
    public PlayerSwordNormalAttackState SwordNormalAttackState { get; private set; }
    public PlayerSwordEnhancedAttackState SwordEnhancedAttackState { get; private set; }
    public PlayerSwordStrongAttackState SwordStrongAttackState { get; private set; }
    public PlayerSwordSkyAttackState SwordSkyAttackState { get; private set; }
    public PlayerSwordCounterAttackState SwordCounterAttackState { get; private set; }
    // public PlayerSwordSoulOneAttackState SwordSoulOneAttackState { get; private set; }
    public PlayerSwordSoulMaxAttackState SwordSoulMaxAttackState { get; private set; }
    public SwordEnhanceState SwordEnhanceState { get; private set; }
    #endregion

    #region GunStates
    public PlayerGunHubState GunHubState { get; private set; }
    public PlayerGunNormalAttackState GunNormalAttackState { get; private set; }
    public PlayerGunChargingState GunChargeAttackState { get; private set; }
    public PlayerGunCounterState GunCounterAttackState { get; private set; }
    public PlayerThrowGrenadeState GunThrowGrenadeState { get; private set; }
    public PlayerGunS3State GunS3State { get; private set; }

    #endregion

    #region FistStates
    public PlayerFistHubState FistHubState { get; private set; }
    public PlayerFistNormalAttackState FistNormalAttackState { get; private set; }
    public PlayerFistStrongAttackState FistStrongAttackState { get; private set; }
    public PlayerFistStaticStrongAttackState FistStaticStrongAttackState { get; private set; }
    public PlayerFistCounterAttackState FistCounterAttackState { get; private set; }
    public PlayerFistS3ChargeState FistS3ChargeState { get; private set; }
    public PlayerFistS3AttackState FistS3AttackState { get; private set; }
    #endregion

    #region Components
    public PlayerTimeSkillManager TimeSkillManager { get; private set; }
    public Core Core { get; private set; }

    private Movement movement;
    private Stats stats;

    public Animator Anim { get; private set; }
    public PlayerWeaponManager WeaponManager { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    [field: SerializeField] public Transform DashDirectionIndicator { get; private set; }
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
        stats = Core.GetCoreComponent<Stats>();
        movement = Core.GetCoreComponent<Movement>();

        TimeSkillManager = GetComponent<PlayerTimeSkillManager>();
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        MovementCollider = GetComponent<BoxCollider2D>();
        WeaponManager = GetComponent<PlayerWeaponManager>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, PlayerData, "idle");
        ChangeSceneState = new PlayerChangeSceneState(this, StateMachine, PlayerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, PlayerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, PlayerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, PlayerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, PlayerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, PlayerData, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, PlayerData, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, PlayerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, PlayerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, PlayerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, PlayerData, "inAir");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, PlayerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, PlayerData, "crouchMove");
        DeadState = new PlayerDeadState(this, StateMachine, PlayerData, "dead");

        BlockState = new PlayerBlockState(this, StateMachine, PlayerData, "block");
        PerfectBlockState = new PlayerPerfectBlockState(this, StateMachine, PlayerData, "perfectBlock");
        RegenState = new PlayerRegenState(this, StateMachine, PlayerData, "regen");

        SwordHubState = new PlayerSwordHubState(this, StateMachine, PlayerData, "swordAttack");
        SwordNormalAttackState = new PlayerSwordNormalAttackState(this, StateMachine, PlayerData, "swordNormalAttack");
        SwordStrongAttackState = new PlayerSwordStrongAttackState(this, StateMachine, PlayerData, "swordStrongAttack");
        SwordSkyAttackState = new PlayerSwordSkyAttackState(this, StateMachine, PlayerData, "swordSkyAttack");
        SwordCounterAttackState = new PlayerSwordCounterAttackState(this, StateMachine, PlayerData, "swordCounterAttack");
        // SwordSoulOneAttackState = new PlayerSwordSoulOneAttackState(this, StateMachine, PlayerData, "swordSoulOneAttack");
        SwordSoulMaxAttackState = new PlayerSwordSoulMaxAttackState(this, StateMachine, PlayerData, "swordSoulMaxAttack");
        SwordEnhanceState = new SwordEnhanceState(this, StateMachine, PlayerData, "swordEnhance");
        SwordEnhancedAttackState = new PlayerSwordEnhancedAttackState(this, StateMachine, PlayerData, "swordEnhancedAttack");

        GunHubState = new PlayerGunHubState(this, StateMachine, PlayerData, "gunAttack");
        GunNormalAttackState = new PlayerGunNormalAttackState(this, StateMachine, PlayerData, "gunNormalAttack");
        GunChargeAttackState = new PlayerGunChargingState(this, StateMachine, PlayerData, "gunChargeAttack");
        GunCounterAttackState = new PlayerGunCounterState(this, StateMachine, PlayerData, "gunCounterAttack");
        GunThrowGrenadeState = new PlayerThrowGrenadeState(this, StateMachine, PlayerData, "gunThrowGrenade");
        GunS3State = new PlayerGunS3State(this, StateMachine, PlayerData, "gunS3Attack");

        FistHubState = new PlayerFistHubState(this, StateMachine, PlayerData, "fistAttack");
        FistNormalAttackState = new PlayerFistNormalAttackState(this, StateMachine, PlayerData, "fistNormalAttack");
        FistStrongAttackState = new PlayerFistStrongAttackState(this, StateMachine, PlayerData, "fistStrongAttack");
        FistStaticStrongAttackState = new PlayerFistStaticStrongAttackState(this, StateMachine, PlayerData, "fistStaticStrongAttack");
        FistCounterAttackState = new PlayerFistCounterAttackState(this, StateMachine, PlayerData, "fistCounterAttack");
        FistS3ChargeState = new PlayerFistS3ChargeState(this, StateMachine, PlayerData, "fistS3Charge");
        FistS3AttackState = new PlayerFistS3AttackState(this, StateMachine, PlayerData, "fistS3Attack");
        
        movement.OrginalGravityScale = PlayerData.gravityScale;

    }

    private void OnEnable()
    {
        stats.Health.OnCurrentValueZero += HandleHealthZero;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        StateMachine.Initialize(ChangeSceneState);

        gameManager.OnChangeSceneGoUp += HandleChangeSceneToUp;
        gameManager.OnChangeSceneGoDown += HandleChangeSceneToDown;
        gameManager.OnChangeSceneGoRight += HandleChangeSceneToRight;
        gameManager.OnChangeSceneGoLeft += HandleChangeSceneToLeft;
        gameManager.OnChangeSceneFinished += HandleChangeSceneFinished;
    }

    private void OnDisable()
    {
        gameManager.OnChangeSceneGoUp -= HandleChangeSceneToUp;
        gameManager.OnChangeSceneGoDown -= HandleChangeSceneToDown;
        gameManager.OnChangeSceneGoRight -= HandleChangeSceneToRight;
        gameManager.OnChangeSceneGoLeft -= HandleChangeSceneToLeft;
        gameManager.OnChangeSceneFinished -= HandleChangeSceneFinished;
    }

    private void Update()
    {
        Core.LogicUpdate();

        StateMachine.CurrentState.LogicUpdate();

    }

    private void FixedUpdate()
    {
        Core.PhysicsUpdate();

        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();
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

    #region Change Scene
    private void HandleChangeSceneToUp()
    {
        StateMachine.ChangeState(ChangeSceneState);
    }
    private void HandleChangeSceneToDown()
    {
        StateMachine.ChangeState(ChangeSceneState);
    }
    private void HandleChangeSceneToRight()
    {
        StateMachine.ChangeState(ChangeSceneState);
    }
    private void HandleChangeSceneToLeft()
    {
        StateMachine.ChangeState(ChangeSceneState);
    }

    private void HandleChangeSceneFinished()
    {
        Invoke(nameof(ChangeToIdleState), 0.15f);
    }
    #endregion
    private void ChangeToIdleState()
    {
        CancelInvoke(nameof(ChangeToIdleState));
        ChangeSceneState.SetCanChangeStateTrue();
    }
    #endregion

    private void HandleHealthZero()
    {
        if(StateMachine.CurrentState != DeadState)
            StateMachine.ChangeState(DeadState);
    }

    public void TeleportPlayer(Vector2 position)
    {
        movement.Teleport(position);
    }

    private void OnDrawGizmos()
    {
        if(PlayerData)
            Gizmos.DrawWireSphere(transform.position, PlayerData.perfectBlockKnockbackRadius);
    }
}


