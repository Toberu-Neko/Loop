using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_NormalAttackState2 : SingleMeleeAttackState
{
    private Boss0New boss;
    public B0N_NormalAttackState2(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        if (boss.NormalAttackState1.DoEnhancedAttack)
        {
            boss.EnterSlowTrigger.SetActive(true);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (boss.NormalAttackState1.DoEnhancedAttack)
        {
            boss.EnterSlowTrigger.SetActive(false);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        boss.NormalAttackState1.SetEndTime(EndTime);
        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
