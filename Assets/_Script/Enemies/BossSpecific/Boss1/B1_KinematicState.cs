using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_KinematicState : KinematicState
{
    private Boss1 boss;
    public B1_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoStunState)
        {
            stateMachine.ChangeState(boss.StunState);
        }
    }
}
