using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity
{
    public E3_IdleState IdleState { get; private set; }
    public E3_MoveState MoveState { get; private set; }
    public E3_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E3_LookForPlayerState LookForPlayerState { get; private set; }
    public E3_MeleeAttackState MeleeAttackState { get; private set; }
    public E3_StunState StunState { get; private set; }
    public E3_DeadState DeadState { get; private set; }
    public E3_ShieldMoveState ShieldMoveState { get; private set; }
    public E3_ChargeState ChargeState { get; private set; }
    public E3_KinematicState KinematicState { get; private set; }


    [SerializeField] private E3_StateData stateData;

    private S_EnemyIdleState idleStateData;
    private S_EnemyGroundMoveState moveStateData;
    private S_EnemyPlayerDetectedState playerDetectedStateData;
    private S_EnemyLookForPlayerState lookForPlayerStateData;
    private S_EnemyMeleeAttackState meleeAttackStateData;
    private S_EnemyStunState stunStateData;
    private S_EnemyDeadState deadStateData;
    private S_EnemyChargeState chargeStateData;

    private S_EnemyShieldMoveState shieldMoveStateData;

    [SerializeField] private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        idleStateData = stateData.idleStateData;
        moveStateData = stateData.groundMoveStateData;
        playerDetectedStateData = stateData.playerDetectedStateData;
        lookForPlayerStateData = stateData.lookForPlayerStateData;
        meleeAttackStateData = stateData.meleeAttackStateData;
        stunStateData = stateData.stunStateData;
        deadStateData = stateData.deadStateData;
        shieldMoveStateData = stateData.shieldMoveStateData;
        chargeStateData = stateData.chargeStateData;

        MoveState = new E3_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E3_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E3_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        LookForPlayerState = new E3_LookForPlayerState(this, StateMachine, "shieldMove", lookForPlayerStateData, this);
        MeleeAttackState = new E3_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E3_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E3_DeadState(this, StateMachine, "dead", deadStateData, this);
        ShieldMoveState = new E3_ShieldMoveState(this, StateMachine, "shieldMove", shieldMoveStateData, this);
        ChargeState = new E3_ChargeState(this, StateMachine, "shieldMove", chargeStateData, this);
        KinematicState = new E3_KinematicState(this, StateMachine, "kinematic", this);
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
