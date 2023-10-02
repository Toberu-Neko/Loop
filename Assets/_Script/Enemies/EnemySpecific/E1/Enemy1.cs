using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState IdleState { get; private set; }
    public E1_MoveState MoveState { get; private set; }
    public E1_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E1_ChargeState ChargeState { get; private set; }
    public E1_LookForPlayerState LookForPlayerState { get; private set; }
    public E1_MeleeAttackState MeleeAttackState { get; private set; }
    public E1_StunState StunState { get; private set; }
    public E1_DeadState DeadState { get; private set; }
    public E1_KinematicState KinematicState { get; private set; }

    [SerializeField] private E1_StateData stateData;

    private ED_EnemyIdleState idleStateData;
    private ED_EnemyGroundMoveState moveStateData;
    private ED_EnemyPlayerDetectedState playerDetectedStateData;
    private ED_EnemyChargeState chargeStateData;
    private ED_EnemyLookForPlayerState lookForPlayerStateData;
    private ED_EnemyMeleeAttackState meleeAttackStateData;
    private ED_EnemyStunState stunStateData;
    private ED_EnemyDeadState deadStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        idleStateData = stateData.idleStateData;
        moveStateData = stateData.groundMoveStateData;
        playerDetectedStateData = stateData.playerDetectedStateData;
        chargeStateData = stateData.chargeStateData;
        lookForPlayerStateData = stateData.lookForPlayerStateData;
        meleeAttackStateData = stateData.meleeAttackStateData;
        stunStateData = stateData.stunStateData;
        deadStateData = stateData.deadStateData;

        MoveState = new E1_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E1_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E1_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        ChargeState = new E1_ChargeState(this, StateMachine, "charge", chargeStateData, this);
        LookForPlayerState = new E1_LookForPlayerState(this, StateMachine, "lookForPlayer", lookForPlayerStateData, this);
        MeleeAttackState = new E1_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E1_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E1_DeadState(this, StateMachine, "dead", deadStateData, this);
        KinematicState = new E1_KinematicState(this, StateMachine, "kinematic", this);
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

        MoveState.Disable();
        IdleState.Disable();
        PlayerDetectedState.Disable();
        ChargeState.Disable();
        LookForPlayerState.Disable();
        MeleeAttackState.Disable();
        StunState.Disable();
        DeadState.Disable();

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
