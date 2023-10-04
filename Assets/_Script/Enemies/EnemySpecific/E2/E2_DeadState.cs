using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DeadState : DeadState
{
    private Enemy2 enemy;
    public E2_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDeadState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName)
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
