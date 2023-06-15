using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected D_DodgeState stateData;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;


    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isGrounded = core.CollisionSenses.Ground;
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        isDodgeOver = false;
        core.Movement.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -core.Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
