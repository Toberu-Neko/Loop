using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_ShieldMoveState : ShieldMoveState
{
    private Enemy3 enemy;
    public E3_ShieldMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyShieldMoveState stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (goToStunState)
        {
            stateMachine.ChangeState(enemy.StunState);
        }
        else if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else if (!isPlayerInMaxAgroRange && !stopMovement)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
    }
}
