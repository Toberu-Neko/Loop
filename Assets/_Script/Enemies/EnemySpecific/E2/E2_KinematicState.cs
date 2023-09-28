using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_KinematicState : KinematicState
{
    private Enemy2 enemy;
    public E2_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy2 enemy) : base(entity, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoStunState)
        {
            stateMachine.ChangeState(enemy.StunState);
        }
    }
}
