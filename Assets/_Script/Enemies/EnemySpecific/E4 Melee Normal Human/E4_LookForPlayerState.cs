using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_LookForPlayerState : LookForPlayerState
{
    private Enemy4 enemy;

    public E4_LookForPlayerState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyLookForPlayerState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            if (enemy.GotoMoveState)
            {
                stateMachine.ChangeState(enemy.MoveState);
            }
            else
            {
                stateMachine.ChangeState(enemy.IdleState);
            }
        }
    }

}
