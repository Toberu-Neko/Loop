using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_PlayerDetectedState : PlayerDetectedState
{
    private Enemy4 enemy;

    public E4_PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyPlayerDetectedState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (performCloseRangeAction && enemy.DodgeState.CheckCanDodge())
        {
            stateMachine.ChangeState(enemy.DodgeState);
        }
        else if (performCloseRangeAction && enemy.MeleeAttackState.CheckCanAttack())
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedMoveState);
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
