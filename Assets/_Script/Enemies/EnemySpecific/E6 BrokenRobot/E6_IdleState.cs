using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_IdleState : IdleState
{
    private Enemy6 enemy;
    public E6_IdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Enemy6 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
