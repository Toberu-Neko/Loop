using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected S_EnemyStunState stateData;

    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    private Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;

    protected Combat Combat => combat ? combat : core.GetCoreComponent<Combat>();
    private Combat combat;

    protected Stats Stats => stats ? stats : core.GetCoreComponent<Stats>();
    private Stats stats;
    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, S_EnemyStunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = CollisionSenses.Ground;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
        Movement.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.LastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();

        entity.ResetStunResistance();
        Stats.ResetPoiseDecreaseable();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }

        if(isGrounded && Time.time >= StartTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            Movement.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
