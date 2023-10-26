using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_AngryState : EnemyWaitForAnimFinishState
{
    private Boss0New boss;
    public B0N_AngryState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0New boss) : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        Stats.IsAngry = true;

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}