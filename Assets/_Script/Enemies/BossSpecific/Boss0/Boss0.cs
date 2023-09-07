using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0 : BossBase
{
    public B0_IdleState IdleState { get; private set; }
    public B0_PlayerDetectedState PlayerDetectedState { get; private set; }
    public B0_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }

    public B0_ChargeState ChargeState { get; private set; }
    public B0_BookmarkState BookmarkState { get; private set; }

    public B0_MeleeAttackState NormalAttackState { get; private set; }
    public B0_MeleeAttackState StrongAttackState { get; private set; }
    public B0_MeleeAttackState MultiAttackState { get; private set; }
    public B0_RangedAttackState RangedAttackState { get; private set; }

    public B0_StunState StunState { get; private set; }
    public B0_DeadState DeadState { get; private set; }

    [SerializeField] private B0_StateData stateData;

    private S_EnemyIdleState idleStateData;
    private S_EnemyPlayerDetectedState playerDetectedStateData;
    private S_PlayerDetectedMoveState playerDetectedMoveStateData;

    private S_EnemyChargeState chargeStateData;
    private S_EnemyBookmarkState bookmarkStateData;

    private S_EnemyMeleeAttackState normalAttackStateData;
    private S_EnemyMeleeAttackState strongAttackStateData;
    private S_EnemyMeleeAttackState multiAttackStateData;
    private S_EnemyRangedAttackState rangedAttackStateData;

    private S_EnemyStunState stunStateData;
    private S_EnemyDeadState deadStateData; 
    
    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();

        idleStateData = stateData.idleStateData;
        playerDetectedStateData = stateData.playerDetectedStateData;
        playerDetectedMoveStateData = stateData.playerDetectedMoveStateData;
        
        chargeStateData = stateData.chargeStateData;
        bookmarkStateData = stateData.bookmarkStateData;
        
        normalAttackStateData = stateData.normalAttackStateData;
        strongAttackStateData = stateData.strongAttackStateData;
        multiAttackStateData = stateData.multiAttackStateData;
        rangedAttackStateData = stateData.rangedAttackStateData;
        
        stunStateData = stateData.stunStateData;
        deadStateData = stateData.deadStateData;


        IdleState = new B0_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new B0_PlayerDetectedState(this, StateMachine, "playerDetected", playerDetectedStateData, this);
        PlayerDetectedMoveState = new B0_PlayerDetectedMoveState(this, StateMachine, "playerDetectedMove", playerDetectedMoveStateData, this);

        ChargeState = new B0_ChargeState(this, StateMachine, "charge", chargeStateData, this);
        BookmarkState = new B0_BookmarkState(this, StateMachine, "bookmark", bookmarkStateData, this);

        NormalAttackState = new B0_MeleeAttackState(this, StateMachine, "normalAttack", meleeAttackPosition, normalAttackStateData, this);
        StrongAttackState = new B0_MeleeAttackState(this, StateMachine, "strongAttack", meleeAttackPosition, strongAttackStateData, this);
        MultiAttackState = new B0_MeleeAttackState(this, StateMachine, "multiAttack", meleeAttackPosition, multiAttackStateData, this);
        RangedAttackState = new B0_RangedAttackState(this, StateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

        StunState = new B0_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new B0_DeadState(this, StateMachine, "dead", deadStateData, this);
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        stats.Stamina.OnCurrentValueZero += HandlePoiseZero;
        stats.Health.OnCurrentValueZero += HandleHealthZero;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        stats.Stamina.OnCurrentValueZero -= HandlePoiseZero;
        stats.Health.OnCurrentValueZero -= HandleHealthZero;
    }

    private void HandlePoiseZero()
    {
        if (stats.Health.CurrentValue <= 0)
            return;

        StateMachine.ChangeState(StunState);
    }

    private void HandleHealthZero() => StateMachine.ChangeState(DeadState);
}
