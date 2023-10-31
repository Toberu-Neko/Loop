using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    protected ED_EnemyIdleState stateData;

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;

    protected float idleTime;

    public IdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityX(0f);
        Combat.OnDamaged += HandleOnDamaged;
        isIdleTimeOver = false;

        SetRandomIdleTime();
    }

    private void HandleOnDamaged()
    {
        if (!isPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= HandleOnDamaged;

        if (flipAfterIdle)
        {
            Movement.Flip();
        }
        flipAfterIdle = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        if (Time.time >= StartTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
    }
    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }
    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
