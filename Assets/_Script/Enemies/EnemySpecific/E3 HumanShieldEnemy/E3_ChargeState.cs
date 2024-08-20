using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_ChargeState : ChargeState
{
    private Enemy3 enemy;
    public E3_ChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyChargeState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.SetNormalBlock(true);
    }

    public override void Exit()
    {
        base.Exit();
        Combat.SetNormalBlock(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (gotoNextState)
        {   
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
