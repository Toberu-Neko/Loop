using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_EnemyPerfectBlockState : EnemyPerfectBlockState
{
    private Boss1 boss;

    public B1_EnemyPerfectBlockState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyPerfectBlockState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoNextState)
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }
}
