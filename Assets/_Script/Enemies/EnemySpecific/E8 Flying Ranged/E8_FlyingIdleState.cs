using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_FlyingIdleState : EnemyFlyingIdleState
{
    private Enemy8 enemy;
    public E8_FlyingIdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Enemy8 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoNextState)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
        else if (CheckPlayerSenses.IsPlayerInMinAgroRange && CheckPlayerSenses.CanSeePlayer)
        {
            stateMachine.ChangeState(enemy.ChooseBulletState);
        }
    }
}
