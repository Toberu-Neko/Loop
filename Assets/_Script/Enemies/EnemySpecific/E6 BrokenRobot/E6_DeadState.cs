using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_DeadState : DeadState
{
    public E6_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Death.Die();
    }
}
