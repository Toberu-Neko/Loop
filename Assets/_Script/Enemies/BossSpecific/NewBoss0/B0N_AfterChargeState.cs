using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_AfterChargeState : EnemyWaitForAnimFinishState
{
    private Boss0New boss;
    public B0N_AfterChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0New boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
