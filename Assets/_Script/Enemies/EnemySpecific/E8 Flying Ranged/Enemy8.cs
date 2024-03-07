using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy8 : Entity
{
    [field: SerializeField] public E8_StateData EnemyData { get; private set; }

    public E8_FlyingIdleState IdleState { get; private set; }
    public E8_FlyingMovementState MoveState { get; private set; }
    public E8_ChooseBulletState ChooseBulletState { get; private set; }
    public E8_SkySingleRangedAttackState RangedAttackState { get; private set; }

    public E8_StunState StunState { get; private set; }
    public E8_KinematicState KinematicState { get; private set; }
    public E8_DeadState DeadState { get; private set; }

    [SerializeField] private Transform attackPosition;

    public override void Awake()
    {
        base.Awake();

        IdleState = new E8_FlyingIdleState(this, StateMachine, "idle", EnemyData.idleStateData, this);
        MoveState = new E8_FlyingMovementState(this, StateMachine, "move", EnemyData.movementStateData, this);
        ChooseBulletState = new E8_ChooseBulletState(this, StateMachine, "chooseBullet", EnemyData.chooseSingleBulletStateData, attackPosition, this);
        RangedAttackState = new E8_SkySingleRangedAttackState(this, StateMachine, "rangedAttack", attackPosition, EnemyData.rangedAttackStateData, this);

        StunState = new E8_StunState(this, StateMachine, "stun", EnemyData.stunStateData, this);
        KinematicState = new E8_KinematicState(this, StateMachine, "kinematic", this);
        DeadState = new E8_DeadState(this, StateMachine, "dead", this);
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

        Combat.SetPerfectBlockAllDir(true);
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
