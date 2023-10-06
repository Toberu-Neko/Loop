using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_RangedAttackState : SingleRangedAttackState
{
    private Boss1 boss;
    public B1_RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
