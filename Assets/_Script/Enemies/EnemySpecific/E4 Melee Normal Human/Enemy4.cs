using System.Collections;
using System.Collections.Generic;
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

    private S_EnemyIdleState idleStateData;
    private S_EnemyGroundMoveState moveStateData;
    private S_EnemyPlayerDetectedState playerDetectedStateData;
    private S_EnemyLookForPlayerState lookForPlayerStateData;
    private S_EnemyMeleeAttackState meleeAttackStateData;
    private S_EnemyStunState stunStateData;
    private S_EnemyDeadState deadStateData;
    private S_PlayerDetectedMoveState detectedPlayerMoveStateData;
    protected S_EnemyDodgeState dodgeStateData;


    [SerializeField] private Transform meleeAttackPosition;

    private float healthDelta;

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
        detectedPlayerMoveStateData = stateData.detectedPlayerMoveStateData;
        dodgeStateData = stateData.dodgeStateData;

        MoveState = new E4_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E4_IdleState(this, StateMachine, "idle", idleStateData, this);
        PlayerDetectedState = new E4_PlayerDetectedState(this, StateMachine, "idle", playerDetectedStateData, this);
        LookForPlayerState = new E4_LookForPlayerState(this, StateMachine, "idle", lookForPlayerStateData, this);
        MeleeAttackState = new E4_MeleeAttackState(this, StateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        StunState = new E4_StunState(this, StateMachine, "stun", stunStateData, this);
        DeadState = new E4_DeadState(this, StateMachine, "dead", deadStateData, this);
        PlayerDetectedMoveState = new E4_PlayerDetectedMoveState(this, StateMachine, "move", detectedPlayerMoveStateData, this);
        DodgeState = new E4_DodgeState(this, StateMachine, "dodge", dodgeStateData, this);
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
        if (Stats.Health.CurrentValue <= 0 && StateMachine.CurrentState == KinematicState)
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
