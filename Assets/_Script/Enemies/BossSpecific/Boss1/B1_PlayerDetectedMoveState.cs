using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        if (Stats.Health.CurrentValuePercentage <= 0.5f && !Stats.IsAngry)
        {
            stateMachine.ChangeState(boss.AngryState);
        }

        if (performCloseRangeAction && (boss.JumpAndMultiAttackState.CanChangeState() || boss.PerfectBlockState.CanChangeState()))
        {
            if (boss.JumpAndMultiAttackState.CanChangeState())
            {
                stateMachine.ChangeState(boss.JumpAndMultiAttackState);
            }
            else if (boss.PerfectBlockState.CanChangeState())
            {
                stateMachine.ChangeState(boss.PerfectBlockState);
            }
        }
        else if (isPlayerInMaxAgroRange && CanChangeState())
        {
            stateMachine.ChangeState(boss.ChooseRandomBulletState);
        }

    }
}
