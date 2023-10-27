using UnityEngine;

public class Boss0New : BossBase
{
    public B0N_Idle IdleState { get; private set; }
    public B0N_AngryState AngryState { get; private set; }
    public B0N_InitAnim InitAnim { get; private set; }

    public B0N_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }

    public B0N_NormalAttackState1 NormalAttackState1 { get; private set; }
    public B0N_NormalAttackState2 NormalAttackState2 { get; private set; }
    public B0N_StrongAttackState StrongAttackState { get; private set; }

    public B0N_PreChargeState PreChargeState { get; private set; }
    public B0N_ChargeState ChargeState { get; private set; }
    public B0N_MultiAttackState MultiAttackState { get; private set; }

    public B0N_StunState StunState { get; private set; }
    public B0N_KinematicState KinematicState { get; private set; }
    public B0N_DeadState DeadState { get; private set; }

    [SerializeField] private B0N_StateData stateData;
    [SerializeField] private Transform meleeAttackPosition;
    

    public override void Awake()
    {
        base.Awake();

        IdleState = new B0N_Idle(this, StateMachine, "idle", stateData.idleStateData, this);
        AngryState = new B0N_AngryState(this, StateMachine, "angry", this);
        InitAnim = new B0N_InitAnim(this, StateMachine, "init", this);

        PlayerDetectedMoveState = new B0N_PlayerDetectedMoveState(this, StateMachine, "move", stateData.detectedMoveStateData, this);
        NormalAttackState1 = new B0N_NormalAttackState1(this, StateMachine, "normalAttack1", meleeAttackPosition, stateData.normalAttack1StateData, this);
        NormalAttackState2 = new B0N_NormalAttackState2(this, StateMachine, "normalAttack2", meleeAttackPosition, stateData.normalAttack2StateData, this);
        StrongAttackState = new B0N_StrongAttackState(this, StateMachine, "strongAttack", meleeAttackPosition, stateData.strongAttackStateData, this);

        PreChargeState = new B0N_PreChargeState(this, StateMachine, "preCharge", this);
        ChargeState = new B0N_ChargeState(this, StateMachine, "charge", stateData.ChargeState, this);
        MultiAttackState = new B0N_MultiAttackState(this, StateMachine, "multiAttack", meleeAttackPosition, stateData.MultiAttackState, this);

        StunState = new B0N_StunState(this, StateMachine, "stun", stateData.stunStateData, this);
        KinematicState = new B0N_KinematicState(this, StateMachine, "stun", this);
        DeadState = new B0N_DeadState(this, StateMachine, "dead", this);
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

        StateMachine.OnChangeState += HandleChangeState;
    }
    private void HandleChangeState()
    {
        if (Stats.Health.CurrentValuePercentage <= 0.5f && !Stats.IsAngry &&
            StateMachine.CurrentState != AngryState)
        {
            StateMachine.ChangeState(AngryState);
        }
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

        StateMachine.OnChangeState -= HandleChangeState;
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
        StateMachine.ChangeState(InitAnim);
    }
}
