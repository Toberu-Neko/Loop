using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_AfterMagic : EnemyWaitForAnimFinishState
{
    private Boss1 boss;

    public B1_AfterMagic(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetRBKinematic();
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if(boss.StateMachine.PreviousState is EnemyFlyingStateBase)
        {
            boss.StateMachine.ChangeState(boss.FlyingIdleState);
        }
        else
        {
            boss.StateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
    }
}
