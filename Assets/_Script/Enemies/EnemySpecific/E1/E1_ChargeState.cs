using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy1 enemy;
    public E1_ChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyChargeState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isDetectingLedge)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if (isDetectingWall)
        {
            stateMachine.ChangeState(enemy.StunState);
        }
        else if (isChargeTimeOver)
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
