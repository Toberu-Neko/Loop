using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_PlayerDetectedState : PlayerDetectedState
{
    private Enemy6 enemy;
    public E6_PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedState stateData, Enemy6 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedMoveState);
        }
    }
}
