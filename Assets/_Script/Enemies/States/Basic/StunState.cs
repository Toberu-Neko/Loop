using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : EnemyState
{
    protected ED_EnemyStunState stateData;

    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;


    public StunState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyStunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = CollisionSenses.Ground;
        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction;
        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
        Movement.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle);
    }

    public override void Exit()
    {
        base.Exit();

        Stats.Stamina.Init();
        Stats.ResetPoiseDecreaseable();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }

        if(isGrounded && Time.time >= StartTime + 0.2f && !isMovementStopped)
        {
            isMovementStopped = true;
            Movement.SetVelocityZero();
        }

        if (isMovementStopped && isGrounded)
        {
            Movement.SetVelocityZero();
        }
    }
}
