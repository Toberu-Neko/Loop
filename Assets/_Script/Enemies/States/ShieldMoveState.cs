using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMoveState : State
{
    S_EnemyShieldMoveState stateData;
    public ShieldMoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, S_EnemyShieldMoveState stateData) : base(entity, stateMachine, animBoolName)
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
        Combat.NormalBlock = true;
        stopMovement = false;
        performCloseRangeAction = false;
        goToStunState = false;
        stopTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.NormalBlock = false;
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

        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

        if (!stopMovement)
        {
            stopMovement = entity.CheckPlayerInCloseRangeAction();
            stopTime = Time.time;
        }

        isDetectingLedge = CollisionSenses.LedgeVertical;
    }
}
