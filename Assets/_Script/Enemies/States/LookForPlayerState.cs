using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : EnemyState
{
    protected ED_EnemyLookForPlayerState stateData;

    protected bool turnImmediately;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;

    protected int amountOfTurnsDone;


    public LookForPlayerState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyLookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
    }

    public override void Enter()
    {
        base.Enter();

        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;
        lastTurnTime = StartTime;
        amountOfTurnsDone = 0;

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        lastTurnTime = Stats.Timer(lastTurnTime);

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();


        if (turnImmediately && !Stats.IsTimeStopped)
        {
            Movement.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            Movement.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }

        if(amountOfTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }

        if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public void SetTurnImmediately(bool flip)
    {
        turnImmediately = flip;
    }
}
