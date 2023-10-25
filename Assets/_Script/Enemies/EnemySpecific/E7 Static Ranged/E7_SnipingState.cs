using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E7_SnipingState : SnipingState
{
    private Enemy7 enemy;

    public E7_SnipingState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemySnipingState stateData, Enemy7 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
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
