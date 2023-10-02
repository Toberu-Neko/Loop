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

        if (isPlayerInMinAgroRange)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

}
