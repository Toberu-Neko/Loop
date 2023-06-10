using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState IdleState { get; private set; }
    public E1_MoveState MoveState { get; private set; }
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;

    public override void Awake()
    {
        base.Awake();

        MoveState = new E1_MoveState(this, StateMachine, "move", moveStateData, this);
        IdleState = new E1_IdleState(this, StateMachine, "idle", idleStateData, this);
        StateMachine.Initialize(MoveState);
    }
}
