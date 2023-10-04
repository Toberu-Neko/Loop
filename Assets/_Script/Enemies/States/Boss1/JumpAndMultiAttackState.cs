using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndMultiAttackState : EnemyState
{
    protected ED_EnemyJumpAndMultiAttackState stateData;

    public JumpAndMultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyJumpAndMultiAttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocity(stateData.jumpForce, stateData.jumpAngle, -Movement.FacingDirection);
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
        Movement.SetRBKinematic();
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        Movement.SetRBDynamic();
        Movement.SetVelocity(15f, Vector2.one, -Movement.FacingDirection);
        //TODO: Shoot four bullets
    }

    public bool CanChangeState()
    {
        return Time.time - EndTime >= stateData.attackCooldown || StartTime == 0;
    }
}
