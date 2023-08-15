using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Entity
{
    public E5_IdleState IdleState { get; private set; }
    public E5_StunState StunState { get; private set; }
    public E5_DeadState DeadState { get; private set; }
    public E5_SnipingState SnipingState { get; private set; }

    [SerializeField] private E5_StateData data;
    [SerializeField] private Transform attackPosition;

    public override void Awake()
    {
        base.Awake();

        IdleState = new E5_IdleState(this, StateMachine, "idle", data.idleStateData, this);
        StunState = new E5_StunState(this, StateMachine, "stun", data.stunStateData, this);
        DeadState = new E5_DeadState(this, StateMachine, "dead", data.deadStateData, this);
        SnipingState = new E5_SnipingState(this, StateMachine, "sniping", attackPosition, data.snipingStateData, this);

        stats.Stamina.OnCurrentValueZero += HandlePoiseZero;
        stats.Health.OnCurrentValueZero += HandleHealthZero;
    }
    protected override void OnDisable()
    {
        base.OnDisable();

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

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }
}
