using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_StunState : StunState
{
    private Boss1 boss;
    public B1_StunState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyStunState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isStunTimeOver)
        {
            if(stateMachine.PreviousState is EnemyFlyingStateBase)
            {
                boss.transform.position = boss.SkyTeleportPos.position;
                stateMachine.ChangeState(boss.FlyingIdleState);
            }
            else
            {
                stateMachine.ChangeState(boss.PlayerDetectedMoveState);
            }
        }
    }
}
