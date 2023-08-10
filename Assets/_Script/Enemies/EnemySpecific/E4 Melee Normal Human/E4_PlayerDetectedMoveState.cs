using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Enemy4 enemy;
    public E4_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_PlayerDetectedMoveState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
