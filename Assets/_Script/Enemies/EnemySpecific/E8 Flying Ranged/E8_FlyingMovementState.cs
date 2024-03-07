using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_FlyingMovementState : EnemyFlyingMovementState
{
    private Enemy8 enemy;
    public E8_FlyingMovementState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FlyingMovementState stateData, Enemy8 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoIdleState)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
        else if (CheckPlayerSenses.IsPlayerInMinAgroRange && CheckPlayerSenses.CanSeePlayer && enemy.ChooseBulletState.CheckCanEnterState())
        {
            stateMachine.ChangeState(enemy.ChooseBulletState);
        }
    }
}
