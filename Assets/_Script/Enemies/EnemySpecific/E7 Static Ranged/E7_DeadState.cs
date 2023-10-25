using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E7_DeadState : DeadState
{
    public E7_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Death.Die();
    }
}
