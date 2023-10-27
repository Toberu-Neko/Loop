using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Boss0New boss;
    public B0N_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedMoveState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isPlayerInMaxAgroRange && boss.ChargeState.CheckCanCharge())
        {
            stateMachine.ChangeState(boss.PreChargeState);
        }
        else if (isPlayerInMaxAgroRange && boss.MultiAttackState.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.MultiAttackState);
        }
        else if (performCloseRangeAction && boss.NormalAttackState1.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.NormalAttackState1);
        }

    }
}
