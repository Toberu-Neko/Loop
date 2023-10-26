using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedIdleState : EnemyState
{
    protected bool gotoMovementState;
    protected bool doCloseRangeAction;
    public PlayerDetectedIdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        gotoMovementState = false;
        doCloseRangeAction = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!CheckPlayerSenses.IsPlayerInCloseRangeAction)
        {
            gotoMovementState = true;
        }
        
        if(CheckPlayerSenses.IsPlayerInCloseRangeAction)
        {
            doCloseRangeAction = true;
        }
    }
}
