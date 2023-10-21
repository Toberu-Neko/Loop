using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_AngryState : EnemyWaitForAnimFinishState
{
    private Boss1 boss;
    public B1_AngryState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetInvincibleTrue();
        
        if(CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();

        Stats.SetInvincibleFalse();
        Stats.IsAngry = true;

        //TODO angry particle on
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
