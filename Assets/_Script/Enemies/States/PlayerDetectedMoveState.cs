using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedMoveState : EnemyState
{
    protected S_PlayerDetectedMoveState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool stopMovement;

    public PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_PlayerDetectedMoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();

        stopMovement = false;
        performCloseRangeAction = false;

        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void DoChecks()
    {
        base.DoChecks(); 
        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;
        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMaxAgroRange)
        {
            Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
        }
    }

}
