using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_InitAnimState : BossInitAnimState
{
    private Boss1 boss1;
    public B1_InitAnimState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss1) : base(entity, stateMachine, animBoolName)
    {
        this.boss1 = boss1;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss1.PlayerDetectedMoveState);
    }
}
