using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_DeadState : DeadState
{
    Enemy5 enemy;
    public E5_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDeadState stateData, Enemy5 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
