using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public PlayerData PlayerData { get;private set; }
    [field: SerializeField] public SO_PlayerSFX PlayerSFX { get; private set; }
    private GameManager gameManager;
    public PlayerStateMachine StateMachine { get; private set; }
    
    #region ControlerStates
    /// <summary>
    /// Player loading animation.
    /// </summary>
    public PlayerTurnOnState TurnOnState { get; private set; }
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

    /// <summary>
    /// Block animation befor block open, can perfect block.
    /// </summary>
    public PlayerPreBlockState PreBlockState { get; private set; }
    public PlayerBlockState BlockState { get; private set; }
    /// <summary>
    /// Play effect and sound when perfect block.
    /// </summary>
    public PlayerPerfectBlockState PerfectBlockState { get; private set; }

    public PlayerRegenState RegenState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    #endregion

    #region SwordStates
    /// <summary>
    /// Check if player is holding attack button.
    /// </summary>
    public PlayerSwordHubState SwordHubState { get; private set; }
    public PlayerSwordNormalAttackState SwordNormalAttackState { get; private set; }
    /// <summary>
    /// Soul one attack, when player have soul one stats.
    /// </summary>
    public PlayerSwordEnhancedAttackState SwordEnhancedAttackState { get; private set; }
    /// <summary>
    /// Hold attack
    /// </summary>
    public PlayerSwordStrongAttackState SwordStrongAttackState { get; private set; }
    public PlayerSwordSkyAttackState SwordSkyAttackState { get; private set; }
    public PlayerSwordCounterAttackState SwordCounterAttackState { get; private set; }
    /// <summary>
    /// Sword alt attack, when player have three energy.
    /// </summary>
    public PlayerSwordSoulMaxAttackState SwordSoulMaxAttackState { get; private set; }
    /// <summary>
    /// Soul one enhance animation.
    /// </summary>
    public SwordEnhanceState SwordEnhanceState { get; private set; }
    #endregion

    #region GunStates
    public PlayerGunHubState GunHubState { get; private set; }
    public PlayerGunNormalAttackState GunNormalAttackState { get; private set; }
    public PlayerGunCounterState GunCounterAttackState { get; private set; }
    public PlayerThrowGrenadeState GunThrowGrenadeState { get; private set; }
    public PlayerGunS3State GunS3State { get; private set; }

    #endregion

    #region FistStates
    /// <summary>
    /// Check if player is holding attack button.
    /// </summary>
    public PlayerFistHubState FistHubState { get; private set; }
    public PlayerFistNormalAttackState FistNormalAttackState { get; private set; }
    public PlayerFistStrongAttackState FistStrongAttackState { get; private set; }
    /// <summary>
    /// Hold S and skill button to static attack.
    /// </summary>
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
    private Combat combat;

    public Animator Anim { get; private set; }
    public PlayerWeaponManager WeaponManager { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    private Color srDefaultColor;

    [field: SerializeField] public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    #endregion

    #region Other Variables
    public bool NoHand { get; private set; }

    private Vector2 v2Workspace;

    public event Action OnDead;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        #region Get Components
        Core = GetComponentInChildren<Core>();
        stats = Core.GetCoreComponent<Stats>();
        movement = Core.GetCoreComponent<Movement>();
        combat = Core.GetCoreComponent<Combat>();

        TimeSkillManager = GetComponent<PlayerTimeSkillManager>();
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        srDefaultColor = SR.color;
        MovementCollider = GetComponent<BoxCollider2D>();
        WeaponManager = GetComponent<PlayerWeaponManager>();
        #endregion

        #region New Player States
        StateMachine = new PlayerStateMachine();

        TurnOnState = new PlayerTurnOnState(this, StateMachine, PlayerData, "turnOn");
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
        DashState = new PlayerDashState(this, StateMachine, PlayerData, "move");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, PlayerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, PlayerData, "crouchMove");
        DeadState = new PlayerDeadState(this, StateMachine, PlayerData, "dead");

        PreBlockState = new PlayerPreBlockState(this, StateMachine, PlayerData, "preBlock");
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
        #endregion
        
        movement.OrginalGravityScale = PlayerData.gravityScale;
    }

    private void OnEnable()
    {
        EnemyCollisionOn();

        #region Event Subscriptions

        stats.Health.OnCurrentValueZero += HandleHealthZero;
        stats.OnInvincibleStart += Stats_OnInvincibleStart;
        stats.Health.OnValueChanged += HandleValueChanged;

        combat.OnPerfectBlock += OnPerfectBlock_SFX;
        combat.OnDamaged += OnDamaged_SFX;
        combat.OnDamaged += OnDamaged_IgnoreEnemy;

        WeaponManager.OnWeaponChanged += HandleWeaponChanged;
        #endregion
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        StateMachine.Initialize(ChangeSceneState);

        #region Event Subscriptions
        // Can't do this in OnEnable because it will be called before Start
        gameManager.OnChangeSceneGoUp += HandleChangeSceneToUp;
        gameManager.OnChangeSceneGoDown += HandleChangeSceneToDown;
        gameManager.OnChangeSceneGoRight += HandleChangeSceneToRight;
        gameManager.OnChangeSceneGoLeft += HandleChangeSceneToLeft;
        gameManager.OnChangeSceneFinished += HandleChangeSceneFinished;
        #endregion

        HandleWeaponChanged();
    }


    private void OnDisable()
    {
        #region Event Unsubscriptions
        gameManager.OnChangeSceneGoUp -= HandleChangeSceneToUp;
        gameManager.OnChangeSceneGoDown -= HandleChangeSceneToDown;
        gameManager.OnChangeSceneGoRight -= HandleChangeSceneToRight;
        gameManager.OnChangeSceneGoLeft -= HandleChangeSceneToLeft;
        gameManager.OnChangeSceneFinished -= HandleChangeSceneFinished;

        stats.Health.OnCurrentValueZero -= HandleHealthZero;
        stats.OnInvincibleStart -= Stats_OnInvincibleStart;
        stats.Health.OnValueChanged -= HandleValueChanged;

        combat.OnPerfectBlock -= OnPerfectBlock_SFX;
        combat.OnDamaged -= OnDamaged_SFX;
        combat.OnDamaged -= OnDamaged_IgnoreEnemy;

        WeaponManager.OnWeaponChanged -= HandleWeaponChanged;
        #endregion
    }

    private void Update()
    {
        Core.LogicUpdate();

        StateMachine.CurrentState.LogicUpdate();

        Anim.SetFloat("yVelocity", movement.CurrentVelocity.y);
        Anim.SetFloat("xVelocity", Mathf.Abs(movement.CurrentVelocity.x));
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

    #region Event Handlers

    /// <summary>
    /// Check if player have hand before getting first weapon.
    /// </summary>
    private void HandleWeaponChanged()
    {
        if (PlayerInventoryManager.Instance.CanUseWeaponCount > 0)
        {
            NoHand = false;
            Anim.SetBool("noHand", NoHand);
        }
        else
        {
            NoHand = true;
            Anim.SetBool("noHand", NoHand);
        }
    }

    /// <summary>
    /// HUD Effect when player health is low.
    /// </summary>
    private void HandleValueChanged()
    {
        if (gameManager == null)
        {
            return;
        }

        if (stats.Health.CurrentValue / stats.Health.MaxValue <= 0.33f)
        {
            gameManager.SetPlayerDanger(true);
        }
        else
        {
            gameManager.SetPlayerDanger(false);
        }
    }

    /// <summary>
    /// Change player color when invincible.
    /// </summary>
    /// <param name="sec"></param>
    private void Stats_OnInvincibleStart(float sec)
    {
        StopCoroutine(InvincibleColorChange(sec));
        StartCoroutine(InvincibleColorChange(sec));
    }

    /// <summary>
    /// Change player color when invincible.
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    private IEnumerator InvincibleColorChange(float sec)
    {

        float alpha = 1f;
        float startTime = Time.time;
        bool isDecreasing = true;

        while (startTime + sec > Time.time)
        {
            if (isDecreasing)
            {
                alpha = Mathf.Lerp(alpha, 0.35f, Time.deltaTime * 12f);
            }
            else
            {
                alpha = Mathf.Lerp(alpha, 1f, Time.deltaTime * 12f);
            }

            if (alpha <= 0.4f)
            {
                isDecreasing = false;
            }
            else if (alpha >= 0.95f)
            {
                isDecreasing = true;
            }

            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, alpha);

            yield return null;
        }
        SR.color = srDefaultColor;
    }

    private void OnDamaged_SFX()
    {
        AudioManager.Instance.PlaySoundFX(PlayerSFX.getHit, transform, AudioManager.SoundType.twoD);
    }

    private void OnPerfectBlock_SFX()
    {
        AudioManager.Instance.PlaySoundFX(PlayerSFX.perfectBlock, transform, AudioManager.SoundType.twoD);
    }

    private void OnDamaged_IgnoreEnemy()
    {
        Physics2D.IgnoreLayerCollision(7, 13, true);
        CancelInvoke(nameof(EnemyCollisionOn));
        Invoke(nameof(EnemyCollisionOn), PlayerData.ignoreEnemyAfterDamagedDuration);
    }

    private void HandleHealthZero()
    {
        if (StateMachine.CurrentState != DeadState)
            StateMachine.ChangeState(DeadState);
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

    private void AnimationSFXTrigger() => StateMachine.CurrentState.AnimationSFXTrigger();

    private void EarlyFinishAnimation() => StateMachine.CurrentState.EarlyFinishAnimation();

    #region Change Scene
    private void HandleChangeSceneToUp()
    {
        StateMachine.ChangeState(ChangeSceneState);
    }
    private void HandleChangeSceneToDown()
    {
        StateMachine.ChangeState(ChangeSceneState);
    }
    public void HandleChangeSceneToRight()
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

    public void HandleDeath()
    {
        OnDead?.Invoke();
    }

    private void EnemyCollisionOn()
    {
        if(StateMachine.CurrentState != DashState)
        {
            Physics2D.IgnoreLayerCollision(7, 13, false);
        }
    }
    
    private void OnDrawGizmos()
    {
        if(PlayerData)
            Gizmos.DrawWireSphere(transform.position, PlayerData.perfectBlockKnockbackRadius);
    }
}


