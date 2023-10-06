using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_JumpAndMultiAttackState : JumpAndMultiAttackState
{
    private Boss1 boss;
    public B1_JumpAndMultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyJumpAndMultiAttackState stateData, Transform attackPos, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData, attackPos)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
