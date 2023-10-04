using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_DeadState : DeadState
{
    public B0_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDeadState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Death.Die();
    }
}
