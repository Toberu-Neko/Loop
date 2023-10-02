using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : EnemyState
{
    protected ED_EnemyDodgeState stateData;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;


    public DodgeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyDodgeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = CollisionSenses.Ground;
        performCloseRangeAction = CheckPlayerSenses.IsPlayerInCloseRangeAction;
        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
    }   

    public override void Enter()
    {
        base.Enter();

        isDodgeOver = false;
        Combat.Knockback(stateData.dodgeAngle, stateData.dodgeSpeed, Movement.ParentTransform.position + Movement.ParentTransform.right, false);
        // Movement.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + 0.2f && isGrounded)
        {
            isDodgeOver = true;
        }
    }

    public bool CheckCanDodge()
    {
        return Time.time >= StartTime + stateData.dodgeCooldown || StartTime == 0;
    }
}
