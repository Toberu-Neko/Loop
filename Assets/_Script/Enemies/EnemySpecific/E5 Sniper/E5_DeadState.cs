using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_DeadState : DeadState
{
    Enemy5 enemy;
    public E5_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy5 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Death.Die();

        stateMachine.ChangeState(enemy.IdleState);
    }

}
