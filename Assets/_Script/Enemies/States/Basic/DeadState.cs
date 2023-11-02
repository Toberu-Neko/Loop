using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyState
{

    public DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Movement.SetCanSetVelocity(false);
        Stats.SetInvincibleTrue();

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();

        Stats.SetInvincibleFalse();
        Movement.SetCanSetVelocity(true);
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
