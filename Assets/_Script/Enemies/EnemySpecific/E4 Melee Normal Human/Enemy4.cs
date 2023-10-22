using UnityEngine;

public class Enemy4 : Entity
{
    public E4_IdleState IdleState { get; private set; }
    public E4_MoveState MoveState { get; private set; }
    public E4_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E4_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }
    public E4_LookForPlayerState LookForPlayerState { get; private set; }
    public E4_MeleeAttackState MeleeAttackState { get; private set; }
    public E4_StunState StunState { get; private set; }
    public E4_DeadState DeadState { get; private set; }
    public E4_DodgeState DodgeState { get; private set; }
    public E4_KinematicState KinematicState { get; private set; }

    [SerializeField] private E4_StateData stateData;
    [SerializeField] private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        MoveState = new E4_MoveState(this, StateMachine, "move", stateData.groundMoveStateData, this);
        IdleState = new E4_IdleState(this, StateMachine, "idle", stateData.idleStateData, this);
        PlayerDetectedState = new E4_PlayerDetectedState(this, StateMachine, "idle", stateData.playerDetectedStateData, this);
        LookForPlayerState = new E4_LookForPlayerState(this, StateMachine, "idle", stateData.lookForPlayerStateData, this);
        MeleeAttackState = new E4_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, stateData.meleeAttackStateData, this);
        StunState = new E4_StunState(this, StateMachine, "stun", stateData.stunStateData, this);
        DeadState = new E4_DeadState(this, StateMachine, "dead", this);
        PlayerDetectedMoveState = new E4_PlayerDetectedMoveState(this, StateMachine, "move", stateData.detectedPlayerMoveStateData, this);
        DodgeState = new E4_DodgeState(this, StateMachine, "dodge", stateData.dodgeStateData, this);
        KinematicState = new E4_KinematicState(this, StateMachine, "kinematic", this);
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

        Combat.OnGoToKinematicState += GotoKinematicState;
        Combat.OnGoToStunState += OnGotoStunState;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        Stats.Stamina.OnCurrentValueZero -= HandlePoiseZero;
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;

        Combat.OnGoToKinematicState -= GotoKinematicState;
        Combat.OnGoToStunState -= OnGotoStunState;
    }
    private void OnGotoStunState()
    {
        if (Stats.Health.CurrentValue > 0)
            StateMachine.ChangeState(StunState);
        else
            StateMachine.ChangeState(DeadState);
    }

    private void GotoKinematicState(float time)
    {
        KinematicState.SetTimer(time);
        StateMachine.ChangeState(KinematicState);
    }
    private void HandlePoiseZero()
    {
        if (Stats.Health.CurrentValue <= 0 || StateMachine.CurrentState == KinematicState)
            return;

        StateMachine.ChangeState(StunState);
    }

    private void HandleHealthZero()
    {
        if (StateMachine.CurrentState == KinematicState)
            return;
        StateMachine.ChangeState(DeadState);
    }

}
