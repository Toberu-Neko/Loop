using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Old boss 0 can be found in the test level.
/// Just for fun.
/// </summary>
public class Boss0 : BossBase
{
    public B0_IdleState IdleState { get; private set; }
    public B0_PlayerDetectedState PlayerDetectedState { get; private set; }
    public B0_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }

    public B0_ChargeState ChargeState { get; private set; }
    public B0_BookmarkState BookmarkState { get; private set; }

    public B0_NormalAttackState NormalAttackState { get; private set; }
    public B0_StrongAttackState StrongAttackState { get; private set; }
    public B0_MultiAttackState MultiAttackState { get; private set; }
    public B0_RangedAttackState RangedAttackState { get; private set; }
    public B0_KinematicState KinematicState { get; private set; }

    public B0_StunState StunState { get; private set; }
    public B0_DeadState DeadState { get; private set; }

    [SerializeField] private B0_StateData stateData;

    private ED_EnemyIdleState idleStateData;
    private ED_PlayerDetectedState playerDetectedStateData;
    private ED_PlayerDetectedMoveState playerDetectedMoveStateData;

    private ED_EnemyChargeState chargeStateData;
    private ED_EnemyBookmarkState bookmarkStateData;

    private ED_EnemyMeleeAttackState normalAttackStateData;
    private ED_EnemyMeleeAttackState strongAttackStateData;
    private ED_EnemyMeleeAttackState multiAttackStateData;
    private ED_EnemyRangedAttackState rangedAttackStateData;

    private ED_EnemyStunState stunStateData;
    private ED_EnemyDeadState deadStateData; 
    
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

        NormalAttackState = new B0_NormalAttackState(this, StateMachine, "normalAttack", meleeAttackPosition, normalAttackStateData, this);
        StrongAttackState = new B0_StrongAttackState(this, StateMachine, "strongAttack", meleeAttackPosition, strongAttackStateData, this);
        MultiAttackState = new B0_MultiAttackState(this, StateMachine, "multiAttack", meleeAttackPosition, multiAttackStateData, this);
        RangedAttackState = new B0_RangedAttackState(this, StateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);

        StunState = new B0_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new B0_DeadState(this, StateMachine, "dead", deadStateData, this);

        KinematicState = new B0_KinematicState(this, StateMachine, "kinematic", this);
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

        if(Stats.Health.CurrentValue <= 0)
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
        StateMachine.ChangeState(PlayerDetectedState);
    }

}
