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

    [SerializeField] private E1_StateData stateData;

    private S_EnemyIdleState idleStateData;
    private S_EnemyGroundMoveState moveStateData;
    private S_EnemyPlayerDetectedState playerDetectedStateData;
    private S_EnemyChargeState chargeStateData;
    private S_EnemyLookForPlayerState lookForPlayerStateData;
    private S_EnemyMeleeAttackState meleeAttackStateData;
    private S_EnemyStunState stunStateData;
    private S_EnemyDeadState deadStateData;

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
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        Stats.Stamina.OnCurrentValueZero -= HandlePoiseZero;
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
    }

    private void HandlePoiseZero()
    {
        if (Stats.Health.CurrentValue <= 0)
            return;

        StateMachine.ChangeState(StunState);
    }

    private void HandleHealthZero() => StateMachine.ChangeState(DeadState);


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(meleeAttackStateData != null)
            Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
