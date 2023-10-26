using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_StunState : StunState
{
    private Boss0New boss;
    public B0N_StunState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyStunState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isStunTimeOver)
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }
}
