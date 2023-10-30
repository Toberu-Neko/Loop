using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : EnemyState
{
    protected ED_EnemyChargeState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool gotoNextState;
    protected bool performCloseRangeAction;
    public ChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;

        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction && !isChargeTimeOver;
    }

    public override void Enter()
    {
        base.Enter();

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

        if (!isChargeTimeOver)
        {
            if (Stats.IsAngry)
            {
                Movement.SetVelocityX(stateData.angryChargeSpeed * Movement.FacingDirection);
            }
            else
            {
                Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
            }
        }
        else
        {
            Movement.SetVelocityX(0f);
        }


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
        return Time.time > EndTime + stateData.chargeCooldown || EndTime == 0;
    }
}
