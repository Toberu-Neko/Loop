using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_PlayerDetectedState : PlayerDetectedState
{
    private Enemy2 enemy;
    public E2_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

        if (performCloseRangeAction)
        {
            if(Time.time >= enemy.DodgeState.StartTime + enemy.DodgeStateData.dodgeCooldown)
            {
                stateMachine.ChangeState(enemy.DodgeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.MeleeAttackState);
            }
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.RangedAttackState);
        }
        else if(!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            core.Movement.Flip();
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
