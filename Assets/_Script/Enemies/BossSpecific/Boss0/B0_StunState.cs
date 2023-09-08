using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_StunState : StunState
{
    private Boss0 boss;
    public B0_StunState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyStunState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStunTimeOver)
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }
}
