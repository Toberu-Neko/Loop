using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_FlyingMovementState : EnemyFlyingMovementState
{
    private Boss1 boss;
    public B1_FlyingMovementState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FlyingMovementState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoIdleState)
        {
            stateMachine.ChangeState(boss.FlyingIdleState);
        }
    }
}
