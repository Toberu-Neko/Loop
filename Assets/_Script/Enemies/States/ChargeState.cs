using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected S_EnemyChargeState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool gotoNextState;
    protected bool performCloseRangeAction;

    private float startTime;
    private bool firstCharge;
    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, S_EnemyChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        firstCharge = true;
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        startTime = Time.time;

        Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
        isChargeTimeOver = false;
        gotoNextState = false;

        entity.SetSkillCollideDamage(true);
    }

    public override void Exit()
    {
        base.Exit();

        entity.SetSkillCollideDamage(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isChargeTimeOver)
            Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
        else
            Movement.SetVelocityX(0f);


        if (Time.time >= StartTime + stateData.chargeTime && !isChargeTimeOver)
        {
            isChargeTimeOver = true;
        }

        if(Time.time >= StartTime + stateData.chargeTime + stateData.finishChargeDelay && !gotoNextState)
        {
            gotoNextState = true;
        }
    }

    public bool CheckCanCharge()
    {
        if (firstCharge)
        {
            firstCharge = false;
            return true;
        }

        return Time.time >= startTime + stateData.chargeCooldown;
    }
}
