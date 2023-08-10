using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_LookForPlayerState : LookForPlayerState
{
    private Enemy4 enemy;

    public E4_LookForPlayerState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyLookForPlayerState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
    public override void Enter()
    {
        base.Enter();

        Combat.SetNormalBlock(true);
    }
    public override void Exit()
    {
        base.Exit();

        Combat.SetNormalBlock(false);
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
