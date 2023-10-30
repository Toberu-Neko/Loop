using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Boss0New boss;

    private bool firstTimeAngry;
    public B0N_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedMoveState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
        firstTimeAngry = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }

        if (!firstTimeAngry && Stats.IsAngry && performCloseRangeAction && boss.NormalAttackState1.CheckCanAttack())
        {
            firstTimeAngry = true;
            stateMachine.ChangeState(boss.PreAngryAttackState);
        }
        else if (isPlayerInMaxAgroRange && boss.ChargeState.CheckCanCharge())
        {
            stateMachine.ChangeState(boss.PreChargeState);
        }
        else if (performCloseRangeAction && boss.MultiAttackState.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.MultiAttackState);
        }
        else if (!Stats.IsAngry && performCloseRangeAction && boss.NormalAttackState1.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.NormalAttackState1);
        }
        else if (Stats.IsAngry && performCloseRangeAction && boss.NormalAttackState1.CheckCanAttack())
        {
            boss.NormalAttackState1.SetDoEnhancedAttack(false);

            if (Random.Range(0f, 1f) <= boss.EnhancedAttackProbability)
            {
                stateMachine.ChangeState(boss.PreAngryAttackState);
            }
            else
            {
                stateMachine.ChangeState(boss.NormalAttackState1);
            }
        }

    }
}
