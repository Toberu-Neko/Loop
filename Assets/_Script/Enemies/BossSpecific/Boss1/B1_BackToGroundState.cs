using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_BackToGroundState : BackToGroundState
{
    private Boss1 boss;
    public B1_BackToGroundState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_BackToIdleState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(gotoNextState)
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        boss.transform.position = boss.GroundTeleportPos.position;
    }
}
