using UnityEngine;

public class Boss1 : BossBase
{
    public B1_IdleState IdleState { get; private set; }
    public B1_InitAnimState InitAnimState { get; private set; }
    public B1_AngryState AngryState { get; private set; }

    public B1_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }
    public B1_FlyingIdleState FlyingIdleState { get; private set; }
    public B1_FlyingMovementState FlyingMovementState { get; private set; }
    public B1_BackToGroundState BackToGroundState { get; private set; }

    public B1_ChooseRandomBulletState ChooseRandomBulletState { get; private set; }
    public B1_RangedAttackState BlueRangedAttackState { get; private set; }
    public B1_RangedAttackState RedRangedAttackState { get; private set; }

    public B1_EnemyPerfectBlockState PerfectBlockState { get; private set; }
    public B1_CounterAttackState CounterAttackState { get; private set; }

    //Skill
    public B1_JumpAndMultiAttackState JumpAndMultiAttackState { get; private set; }
    public B1_FourSkyAttackState FourSkyAttackState { get; private set; }
    public B1_SliceRoomAndExplodeState SliceRoomAndExplodeState { get; private set; }
    public B1_AbovePlayerAttackState AbovePlayerAttackState { get; private set; }

    public B1_StunState StunState { get; private set; }
    public B1_KinematicState KinematicState { get; private set; }
    public B1_DeadState DeadState { get; private set; }

    [field: SerializeField] public B1_StateData StateData { get; private set; }

    [Header("Positions")]
    [SerializeField] private Transform rangedAttackPosition;
    [SerializeField] private Transform jumpAttackPosition;

    [field: SerializeField] public Transform SkyTeleportPos { get; private set; }
    [field: SerializeField] public Transform GroundTeleportPos { get; private set; }
    [field: SerializeField] public BoxCollider2D BossRoomCollider { get; private set; }
    [SerializeField] private GameObject exitTP;

    public override void Awake()
    {
        base.Awake();

        IdleState = new B1_IdleState(this, StateMachine, "idle", StateData.idleStateData, this);
        InitAnimState = new B1_InitAnimState(this, StateMachine, "init", this);
        AngryState = new B1_AngryState(this, StateMachine, "angry", this);

        PlayerDetectedMoveState = new B1_PlayerDetectedMoveState(this, StateMachine, "move", StateData.playerDetectedMoveStateData, this);
        FlyingIdleState = new B1_FlyingIdleState(this, StateMachine, "idle", StateData.flyingIdleStateData, this);
        FlyingMovementState = new B1_FlyingMovementState(this, StateMachine, "move", StateData.flyingMovementStateData, this);
        BackToGroundState = new B1_BackToGroundState(this, StateMachine, "backToGround", StateData.backToIdleStateData, this);

        ChooseRandomBulletState = new B1_ChooseRandomBulletState(this, StateMachine, "chooseRandomBullet", StateData.chooseRandomBulletStateData, this);
        BlueRangedAttackState = new B1_RangedAttackState(this, StateMachine, "blueRangedAttack", rangedAttackPosition, StateData.blueRangedAttackStateData, this);
        RedRangedAttackState = new B1_RangedAttackState(this, StateMachine, "redRangedAttack", rangedAttackPosition, StateData.redRangedAttackStateData, this);

        PerfectBlockState = new B1_EnemyPerfectBlockState(this, StateMachine, "perfectBlock", StateData.perfectBlockStateData, this);
        CounterAttackState = new B1_CounterAttackState(this, StateMachine, "counterAttack", this);

        JumpAndMultiAttackState = new B1_JumpAndMultiAttackState(this, StateMachine, "jumpAndMultiAttack", StateData.jumpAndMultiAttackStateData, jumpAttackPosition,  this);
        FourSkyAttackState = new B1_FourSkyAttackState(this, StateMachine, "counterAttack", StateData.fourSkyAttackStateData, this); //TODO: Anim
        SliceRoomAndExplodeState = new B1_SliceRoomAndExplodeState(this, StateMachine, "sliceRoomAndExplode", StateData.sliceRoomAndExplodeStateData, BossRoomCollider, rangedAttackPosition, this);
        AbovePlayerAttackState = new B1_AbovePlayerAttackState(this, StateMachine, "abovePlayerAttack", StateData.abovePlayerAttackStateData, this);

        StunState = new B1_StunState(this, StateMachine, "stun", StateData.stunStateData, this);
        KinematicState = new B1_KinematicState(this, StateMachine, "stun", this);
        DeadState = new B1_DeadState(this, StateMachine, "dead", this);

        exitTP.SetActive(false);
    }
    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        Stats.Stamina.OnCurrentValueZero += HandlePoiseZero;
        Stats.Health.OnCurrentValueZero += HandleHealthZero;
        OnEnterBossRoom += HandleEnterBossRoom;

        Combat.OnGoToKinematicState += GotoKinematicState;
        Combat.OnGoToStunState += OnGotoStunState;

        OnAlreadyDefeated += HandleAlreadyDefeated;
    }

    private void HandleAlreadyDefeated()
    {
        exitTP.SetActive(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        Stats.Stamina.OnCurrentValueZero -= HandlePoiseZero;
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
        OnEnterBossRoom -= HandleEnterBossRoom;

        Combat.OnGoToKinematicState -= GotoKinematicState;
        Combat.OnGoToStunState -= OnGotoStunState;

        OnAlreadyDefeated -= HandleAlreadyDefeated;
    }

    private void GotoKinematicState(float time)
    {
        KinematicState.SetTimer(time);
        StateMachine.ChangeState(KinematicState);
    }

    private void OnGotoStunState()
    {
        if (Stats.Health.CurrentValue > 0)
            StateMachine.ChangeState(StunState);
        else
            StateMachine.ChangeState(DeadState);
    }

    private void HandlePoiseZero()
    {
        if (Stats.Health.CurrentValue <= 0 || StateMachine.CurrentState == KinematicState)
            return;

        if (Stats.Health.CurrentValue <= 0)
        {
            StateMachine.ChangeState(DeadState);
        }

        StateMachine.ChangeState(StunState);
    }

    private void HandleHealthZero()
    {
        if (StateMachine.CurrentState == KinematicState)
            return;

        StateMachine.ChangeState(DeadState);
        HandleAlreadyDefeated();
    }

    private new void HandleEnterBossRoom()
    {
        StateMachine.ChangeState(InitAnimState);
    }

}
