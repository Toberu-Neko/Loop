using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_DodgeState : DodgeState
{
    private Enemy2 enemy;
    public E2_DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, S_EnemyDodgeState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDodgeOver)
        {
            if(isPlayerInMaxAgroRange && performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.MeleeAttackState);
            }
            else if (isPlayerInMaxAgroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.RangedAttackState);
            }
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
