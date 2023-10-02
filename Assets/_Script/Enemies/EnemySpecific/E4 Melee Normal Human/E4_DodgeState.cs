using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_DodgeState : DodgeState
{
    private Enemy4 enemy;
    public E4_DodgeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDodgeState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
                stateMachine.ChangeState(enemy.PlayerDetectedMoveState);
            }
            else if (!isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }
}
