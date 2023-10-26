using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_KinematicState : KinematicState
{
    private Boss0 boss;
    public B0_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0 boss) : base(entity, stateMachine, animBoolName)
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
