using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    private Enemy1 enemy;

    public E1_PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyPlayerDetectedState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction && enemy.MeleeAttackState.CheckCanAttack())
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            Movement.Flip();
            stateMachine.ChangeState(enemy.MoveState);
        }
    }
}
