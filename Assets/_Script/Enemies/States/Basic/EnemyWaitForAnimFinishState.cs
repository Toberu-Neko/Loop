using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaitForAnimFinishState : EnemyState
{
    public EnemyWaitForAnimFinishState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }
}
