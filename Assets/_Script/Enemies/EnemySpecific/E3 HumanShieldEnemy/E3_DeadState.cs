using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_DeadState : DeadState
{
    private Enemy3 enemy;

    public E3_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDeadState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ChangeState(enemy.IdleState);

        Death.Die();
    }
}
