using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Enemy6 enemy;

    public E6_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedMoveState stateData, Enemy6 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isPlayerInMaxAgroRange && CanChangeState())
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }


}
