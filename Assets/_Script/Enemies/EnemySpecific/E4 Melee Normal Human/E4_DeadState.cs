using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_DeadState : DeadState
{
    private Enemy4 enemy;
    public E4_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDeadState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(enemy.IdleState);

        Death.Die();
    }
}
