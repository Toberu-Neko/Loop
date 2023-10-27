using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAngryState : EnemyState
{
    public BossAngryState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Stats.SetInvincibleTrue();

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }
    public override void Exit()
    {
        base.Exit();

        Stats.SetInvincibleFalse();
        Stats.IsAngry = true;

        //TODO angry particle on
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }
}
