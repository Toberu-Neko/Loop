using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_NormalAttackState1 : SingleMeleeAttackState
{
    private Boss0New boss;
    public B0N_NormalAttackState1(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (isPlayerInCloseActionRange)
        {
            stateMachine.ChangeState(boss.NormalAttackState2);
        }
        else
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }

    public void SetEndTime(float time)
    {
        EndTime = time;
    }
}
