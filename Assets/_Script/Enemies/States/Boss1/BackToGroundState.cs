using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToGroundState : EnemyState
{
    private ED_BackToIdleState stateData;

    protected bool gotoNextState;
    public BackToGroundState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_BackToIdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();

        gotoNextState = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + stateData.stunTime)
        {
            gotoNextState = true;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        StartTime = Time.time;
    }
}
