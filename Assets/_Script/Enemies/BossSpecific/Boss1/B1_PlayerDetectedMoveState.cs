using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Boss1 boss;
    public B1_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedMoveState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }

        if (performCloseRangeAction && (boss.JumpAndMultiAttackState.CanChangeState()))
        {
            stateMachine.ChangeState(boss.JumpAndMultiAttackState);
        }
        else if (isPlayerInMaxAgroRange && (CanChangeState() || performCloseRangeAction))
        {
            stateMachine.ChangeState(boss.ChooseRandomBulletState);
        }

    }
}
