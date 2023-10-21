using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToGroundState : EnemyState
{
    private ED_BackToIdleState stateData;

    private float stunTime;
    protected bool startStun;
    protected bool gotoNextState;
    public BackToGroundState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_BackToIdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
        Movement.SetRBKinematic();

        gotoNextState = false;
        startStun = false;
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();

        if(Time.time >= stunTime + stateData.stunTime && startStun)
        {
            gotoNextState = true;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        Movement.SetRBDynamic();
        startStun = true;
        stunTime = Time.time;
    }
}
