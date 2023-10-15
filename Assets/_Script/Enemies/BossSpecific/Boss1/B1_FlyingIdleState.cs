using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_FlyingIdleState : EnemyFlyingIdleState
{
    private Boss1 boss;
    public B1_FlyingIdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoNextState)
        {
            if(boss.FlyingMovementState.RemainMoveCount == 0)
            {
                boss.FlyingMovementState.ResetMoveCount();
                //TODO: Go to attack state
                if (!boss.FourSkyAttackState.IsAttackDone)
                {
                    stateMachine.ChangeState(boss.FourSkyAttackState);
                }
                else
                {
                    boss.FourSkyAttackState.ResetAttack();
//                     stateMachine.ChangeState(boss.BackToGroundState);
                }
            }
            else
            {
                stateMachine.ChangeState(boss.FlyingMovementState);
            }
        }
    }
}
