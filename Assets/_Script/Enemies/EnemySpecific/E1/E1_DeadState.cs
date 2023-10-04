using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_DeadState : DeadState
{
    private Enemy1 enemy;

    public E1_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDeadState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName)
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
