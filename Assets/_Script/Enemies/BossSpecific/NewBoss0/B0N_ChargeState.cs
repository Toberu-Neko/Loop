using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_ChargeState : ChargeState
{
    private Boss0New boss;
    public B0N_ChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyChargeState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(boss.StrongAttackState);
        }

        else if (gotoNextState)
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }


}
