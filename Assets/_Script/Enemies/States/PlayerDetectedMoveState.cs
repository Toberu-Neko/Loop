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
    }

    public override void Exit()
    {
        base.Exit();

        entity.Anim.SetBool("detectedIdle", false);
    }

    public override void DoChecks()
    {
        base.DoChecks(); 
        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront || CollisionSenses.WallFrontHead;
        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if ((isPlayerInMaxAgroRange && CollisionSenses.Ground && !performCloseRangeAction) || Time.time <= StartTime + stateData.minMovementTime)
        {
            entity.Anim.SetBool("detectedIdle", false);
            Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
        }
        else
        {
            entity.Anim.SetBool("detectedIdle", true);
            Movement.SetVelocityX(0f);
        }
    }

    public bool CanChangeState()
    {
        return Time.time >= StartTime + Random.Range(stateData.minInStateTime, stateData.maxInStateTime) && Time.time >= StartTime + stateData.minMovementTime;
    }

}
