using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E7_IdleState : IdleState
{
    private Enemy7 enemy;
    public E7_IdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Enemy7 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
    }
}
