using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : EnemyState
{
    protected S_EnemyLookForPlayerState stateData;

    protected bool turnImmediately;
    protected bool isPlayerInMinAgroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;

    protected int amountOfTurnsDone;


    public LookForPlayerState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyLookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
    }

    public override void Enter()
    {
        base.Enter();

        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;
        lastTurnTime = StartTime;
        amountOfTurnsDone = 0;

        Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();

        if (Stats.IsTimeStopped)
        {
            lastTurnTime += Time.deltaTime;
        }
        if (Stats.IsTimeSlowed)
        {
            lastTurnTime += Time.deltaTime * (1f- Stats.TimeSlowMultiplier);
        }

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
