using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_DeadState : DeadState
{
    private Boss1 boss;
    public B1_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Death.Die();
    }
}
