using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingStateBase : EnemyState
{
    public EnemyFlyingStateBase(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetRBKinematic();
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
    }
}
