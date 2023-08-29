using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyState
{
    protected S_EnemyDeadState stateData;

    public DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyDeadState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();

        Movement.SetCanSetVelocity(false);
        Movement.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
