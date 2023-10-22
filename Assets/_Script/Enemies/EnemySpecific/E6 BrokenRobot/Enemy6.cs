using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Entity
{
    public E6_IdleState IdleState { get; private set; }
    public E6_PlayerDetectedState PlayerDetectedState { get; private set; }
    public E6_PlayerDetectedMoveState PlayerDetectedMoveState { get; private set; }
    public E6_DeadState DeadState { get; private set; }

    [SerializeField] private E6_StateData stateData;

    public override void Awake()
    {
        base.Awake();

        IdleState = new E6_IdleState(this, StateMachine, "idle", stateData.idleStateData, this);
        PlayerDetectedState = new E6_PlayerDetectedState(this, StateMachine, "move", stateData.playerDetectedState, this);
        PlayerDetectedMoveState = new E6_PlayerDetectedMoveState(this, StateMachine, "move", stateData.detectedPlayerMoveStateData, this);

        DeadState = new E6_DeadState(this, StateMachine, "dead");
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Stats.Health.OnCurrentValueZero += HandleHealthZero;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        StateMachine.ChangeState(IdleState);

        Stats.Health.OnCurrentValueZero -= HandleHealthZero;
    }

    private void HandleHealthZero()
    {
        StateMachine.ChangeState(DeadState);
    }

}
