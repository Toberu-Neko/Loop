using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_PreMove : EnemyWaitForAnimFinishState
{
    private Enemy8 enemy;
    public E8_PreMove(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy8 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(enemy.MoveState);
    }
}
