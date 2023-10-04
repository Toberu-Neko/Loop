using UnityEngine;

public class Boss1 : BossBase
{
    public B1_IdleState IdleState { get; private set; }
    public B1_InitAnimState InitAnimState { get; private set; }

    public B1_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }
    public B1_JumpAndMultiAttackState JumpAndMultiAttackState { get; private set; }


    public B1_StunState StunState { get; private set; }
    public B1_KinematicState KinematicState { get; private set; }
    public B1_DeadState DeadState { get; private set; }

    [field: SerializeField] public B1_StateData StateData { get; private set; }
    [SerializeField] private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        IdleState = new B1_IdleState(this, StateMachine, "idle", StateData.idleStateData, this);
        InitAnimState = new B1_InitAnimState(this, StateMachine, "init", this);

        PlayerDetectedMoveState = new B1_PlayerDetectedMoveState(this, StateMachine, "move", StateData.playerDetectedMoveStateData, this);
        JumpAndMultiAttackState = new B1_JumpAndMultiAttackState(this, StateMachine, "jumpAndMultiAttack", StateData.jumpAndMultiAttackStateData, this);

        StunState = new B1_StunState(this, StateMachine, "stun", StateData.stunStateData, this);
        KinematicState = new B1_KinematicState(this, StateMachine, "stun", this);
        DeadState = new B1_DeadState(this, StateMachine, "dead", this);
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
    }

    private new void HandleEnterBossRoom()
    {
        StateMachine.ChangeState(InitAnimState);
    }

}
