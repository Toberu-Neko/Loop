using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EnemyState
{
    protected S_EnemyPlayerDetectedState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;

    public PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyPlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        if(CollisionSenses.Ground)  
            Movement.SetVelocityZero();

        performLongRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();

        if (Time.time >= StartTime + stateData.delayTime) 
        {
            performLongRangeAction = true;
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
        isDetectingLedge = CollisionSenses.LedgeVertical;
        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction;
    }
}
