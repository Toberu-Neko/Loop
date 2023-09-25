using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_IdleState : IdleState
{
    private Enemy5 enemy;
    public E5_IdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyIdleState stateData, Enemy5 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float angle;
        if (CheckPlayerSenses.IsPlayerInMaxAgroRange)
        {
            angle = Vector2.Angle((Vector2)(CheckPlayerSenses.IsPlayerInMaxAgroRange.collider.transform.position - Movement.ParentTransform.position).normalized , Movement.ParentTransform.right);
            // Debug.Log("ID Angle: " + angle);
        }
        else
        {
            angle = 180;
        }

        if (CheckPlayerSenses.CanSeePlayer && isPlayerInMaxAgroRange && angle < 30f)
        {
            stateMachine.ChangeState(enemy.SnipingState);
        }
        else if (isIdleTimeOver)
        {
            enemy.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
