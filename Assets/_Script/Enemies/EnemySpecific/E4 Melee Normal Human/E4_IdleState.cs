using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_IdleState : IdleState
{
    private Enemy4 enemy;
    public E4_IdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedState);
        }
        else if (isIdleTimeOver && enemy.GotoMoveState)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
}
