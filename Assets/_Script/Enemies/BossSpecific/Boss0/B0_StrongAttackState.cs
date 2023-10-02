using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_StrongAttackState : StrongAttackState
{
    private Boss0 boss;
    public B0_StrongAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Exit()
    {
        base.Exit();

        if (boss.BookmarkState.isBookmarkActive)
        {
            boss.BookmarkState.ResetBookmark();
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (!isPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }


        if (boss.BookmarkState.isBookmarkActive)
        {
            Movement.Teleport(boss.BookmarkState.GetBookmarkPosition());
            boss.BookmarkState.ResetBookmark();
        }

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }



}
