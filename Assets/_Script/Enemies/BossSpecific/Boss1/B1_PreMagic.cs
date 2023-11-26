using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_PreMagic : EnemyWaitForAnimFinishState
{
    private Boss1 boss;
    private EnemyState nextState;

    public B1_PreMagic(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss1 boss) : base(entity, stateMachine, animBoolName)
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

    public void SetNextState(EnemyState nextState)
    {
        this.nextState = nextState;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        boss.StateMachine.ChangeState(nextState);
    }
}
