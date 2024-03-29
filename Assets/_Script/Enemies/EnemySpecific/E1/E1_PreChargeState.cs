using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PreChargeState : EnemyWaitForAnimFinishState
{
    private Enemy1 enemy;
    public E1_PreChargeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityX(0f);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(enemy.ChargeState);
    }
}
