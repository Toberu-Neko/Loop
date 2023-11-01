using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (Stats.Health.CurrentValuePercentage <= 0.5f && !Stats.IsAngry)
        {
            stateMachine.ChangeState(boss.AngryState);
        }
        else if (Stats.IsAngry && boss.AngryMagicState.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.AngryMagicState);
        }
        else if (isPlayerInMaxAgroRange && boss.ChargeState.CheckCanCharge())
        {
            stateMachine.ChangeState(boss.PreChargeState);
        }
        else if (performCloseRangeAction && boss.MultiAttackState.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.MultiAttackState);
        }
        else if (performCloseRangeAction && boss.NormalAttackState1.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.NormalAttackState1);
        }
    }
}
