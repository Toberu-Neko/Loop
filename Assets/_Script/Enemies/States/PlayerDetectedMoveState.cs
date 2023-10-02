using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedMoveState : EnemyState
{
    protected ED_PlayerDetectedMoveState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;

    public PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_PlayerDetectedMoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();

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
            if(CollisionSenses.Ground)
                Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);

        }
    }

}
