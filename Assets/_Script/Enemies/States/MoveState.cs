using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : EnemyState
{
    protected ED_EnemyGroundMoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;


    public MoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyGroundMoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;
        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
    }
}
