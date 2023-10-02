using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_SnipingState : SnipingState
{
    private Enemy5 enemy;

    public E5_SnipingState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemySnipingState stateData, Enemy5 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (goToIdleState)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
