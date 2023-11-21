using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_AfterMultiAttackState : EnemyWaitForAnimFinishState
{
    private Boss1 boss;
    public B1_AfterMultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
