using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_DeadState : DeadState
{
    private Boss0 boss;
    public B0_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyDeadState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Death.Die();
    }
}
