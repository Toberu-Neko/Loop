using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_KinematicState : KinematicState
{
    private Boss0New boss;
    public B0N_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0New boss) : base(entity, stateMachine, animBoolName)
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
