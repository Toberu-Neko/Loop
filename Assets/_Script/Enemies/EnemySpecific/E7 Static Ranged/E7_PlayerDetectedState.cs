using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E7_PlayerDetectedState : PlayerDetectedState
{
    private Enemy7 enemy;
    public E7_PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedState stateData, Enemy7 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.SnipingState);
        }
    }
}
