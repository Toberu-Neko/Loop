using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_IdleState IdleState { get; private set; }
    public E2_MoveState MoveState { get; private set; }
    public E2_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E2_MeleeAttackState MeleeAttackState { get; private set; }
    public E2_LookForPlayerState LookForPlayerState { get; private set; }
    public E2_StunState StunState { get; private set; }
    public E2_DeadState DeadState { get; private set; }
    public E2_DodgeState DodgeState { get; private set; }
    public E2_RangedAttackState RangedAttackState { get; private set; }
    public E2_KinematicState KinematicState { get; private set; }


    [SerializeField] private E2_StateData stateData;

    private S_EnemyIdleState idleStateData;
    private S_EnemyGroundMoveState moveStateData;
    private S_EnemyPlayerDetectedState playerDetectedStateData;
    private S_EnemyMeleeAttackState meleeAttackStateData;
    private S_EnemyLookForPlayerState lookForPlayerStateData;
    private S_EnemyStunState stunStateData;
    private S_EnemyDeadState deadStateData;
    private S_EnemyDodgeState dodgeStateData;
    private S_EnemyRangedAttackState rangedAttackStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        idleStateData = stateData.idleStateData;
        moveStateData = stateData.groundMoveStateData;
        playerDetectedStateData = stateData.playerDetectedStateData;
        meleeAttackStateData = stateData.meleeAttackStateData;
        lookForPlayerStateData = stateData.lookForPlayerStateData;
        stunStateData = stateData.stunStateData;
        deadStateData = stateData.deadStateData;
        dodgeStateData = stateData.dodgeStateData;
        rangedAttackStateData = stateData.rangedAttackStateData;

        IdleState = new E2_IdleState(this, StateMachine, "idle", idleStateData, this);
        MoveState = new E2_MoveState(this, StateMachine, "move", moveStateData, this);
        PlayerDetectedState = new E2_PlayerDetectedState(this, StateMachine, "idle", playerDetectedStateData, this);
        MeleeAttackState = new E2_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        LookForPlayerState = new E2_LookForPlayerState(this, StateMachine, "idle", lookForPlayerStateData, this);
        StunState = new E2_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E2_DeadState(this, StateMachine, "dead", deadStateData, this);
        DodgeState = new E2_DodgeState(this, StateMachine, "dodge", dodgeStateData, this);
        RangedAttackState = new E2_RangedAttackState(this, StateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);
        KinematicState = new E2_KinematicState(this, StateMachine, "kinematic", this);

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
    private void GotoKinematicState(float time)
    {
        KinematicState.SetTimer(time);
        StateMachine.ChangeState(KinematicState);
    }
    private void OnGotoStunState()
    {
        StateMachine.ChangeState(StunState);
    }

    private void HandlePoiseZero()
    {
        if (Stats.Health.CurrentValue <= 0 && StateMachine.CurrentState != KinematicState)
            return;

        StateMachine.ChangeState(StunState);
    }

    private void HandleHealthZero() => StateMachine.ChangeState(DeadState);

}
