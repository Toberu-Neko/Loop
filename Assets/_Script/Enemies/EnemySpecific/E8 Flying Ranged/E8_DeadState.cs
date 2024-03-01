using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_DeadState : DeadState
{
    public Enemy8 enemy8;
    public E8_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy8 enemy8) : base(entity, stateMachine, animBoolName)
    {
        this.enemy8 = enemy8;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Death.Die();

        stateMachine.ChangeState(enemy8.IdleState);
    }

}
