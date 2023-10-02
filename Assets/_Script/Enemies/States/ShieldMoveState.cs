using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMoveState : EnemyState
{
    ED_EnemyShieldMoveState stateData;
    public ShieldMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyShieldMoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    protected bool isPlayerInMaxAgroRange;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;

    protected bool stopMovement;
    protected float stopTime;

    protected bool goToStunState;
    
    public override void Enter()
    {
        base.Enter();

        // TODO: Perfect block???
        // Combat.PerfectBlock = true;
        Combat.SetNormalBlock(true);
        stopMovement = false;
        performCloseRangeAction = false;
        goToStunState = false;
        stopTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.SetNormalBlock(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (stopMovement)
        {
            Movement.SetVelocityX(0f);

            if(Time.time >= stopTime + stateData.removeShieldTime)
            {
                performCloseRangeAction = true;
            }
        }
        else
        {
            Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;

        if (!stopMovement)
        {
            stopMovement = CheckPlayerSenses.IsPlayerInCloseRangeAction;
            stopTime = Time.time;
        }

        isDetectingLedge = CollisionSenses.LedgeVertical;
    }
}
