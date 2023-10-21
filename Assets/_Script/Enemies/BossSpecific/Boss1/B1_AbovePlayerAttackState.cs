using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_AbovePlayerAttackState : AbovePlayerAttackState
{
    private Boss1 boss;
    public B1_AbovePlayerAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_AbovePlayerAttackState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAttackDone)
        {
            stateMachine.ChangeState(boss.FlyingIdleState);
        }
    }
}
