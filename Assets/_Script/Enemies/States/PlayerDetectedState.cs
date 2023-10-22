using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EnemyState
{
    protected ED_PlayerDetectedState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;

    private float randomDelayTime;

    public PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        if(CollisionSenses.Ground)  
            Movement.SetVelocityZero();

        performLongRangeAction = false;
        randomDelayTime = Random.Range(stateData.minDelayTime, stateData.maxDelayTime);
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

        if (Time.time >= StartTime + randomDelayTime) 
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
