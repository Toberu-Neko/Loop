using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_PlayerDetectedState : PlayerDetectedState
{
    private Enemy3 enemy;

    public E3_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, S_EnemyPlayerDetectedState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
            if (enemy.ChargeState.CheckCanCharge())
            {
                stateMachine.ChangeState(enemy.ChargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.ShieldMoveState);
            }
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
