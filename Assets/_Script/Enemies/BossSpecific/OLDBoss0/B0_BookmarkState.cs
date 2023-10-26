using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_BookmarkState : EnemyBookmarkState
{
    private Boss0 boss;

    public B0_BookmarkState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyBookmarkState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();


        stateMachine.ChangeState(boss.ChargeState);
    }

}
