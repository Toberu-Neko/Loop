using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Boss0 boss;
    public B0_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_PlayerDetectedMoveState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!isPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }

        if (Time.time < StartTime + 2f && !performCloseRangeAction)
        {
            return;
        }

        if (performCloseRangeAction && boss.MultiAttackState.CheckCanAttack())
        {
            Debug.Log("Skill!");
            stateMachine.ChangeState(boss.MultiAttackState);
        }
        else if (isPlayerInMinAgroRange && boss.ChargeState.CheckCanCharge() && ReturnHealthPercentage() > 0.5f)
        {
            stateMachine.ChangeState(boss.ChargeState);
        }
        else if(isPlayerInMinAgroRange && boss.ChargeState.CheckCanCharge() && ReturnHealthPercentage() < 0.5f)
        {
            stateMachine.ChangeState(boss.BookmarkState);
        }
        else if (isPlayerInMaxAgroRange && boss.RangedAttackState.CheckCanAttack())
        {
            stateMachine.ChangeState(boss.RangedAttackState);
        }
        else if (performCloseRangeAction)
        {
            if(ReturnHealthPercentage() < 0.5f)
            {
                switch(Random.Range(0, 3))
                {
                    case 0:
                        stateMachine.ChangeState(boss.StrongAttackState);
                        break;
                    case 1:
                        stateMachine.ChangeState(boss.MultiAttackState);
                        break;
                    case 2:
                        stateMachine.ChangeState(boss.NormalAttackState);
                        break;
                    default:
                        Debug.Log("Random is wrong in B0_PlayerDetectedMoveState");
                        break;
                }
            }
            else
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        stateMachine.ChangeState(boss.NormalAttackState);
                        break;
                    case 1:
                        stateMachine.ChangeState(boss.StrongAttackState);
                        break;
                    default:
                        Debug.Log("Random is wrong in B0_PlayerDetectedMoveState");
                        break;
                }
            }
        }
    }
}
