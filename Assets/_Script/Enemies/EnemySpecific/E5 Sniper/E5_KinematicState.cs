using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_KinematicState : KinematicState
{
    private Enemy5 enemy;
    public E5_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy5 enemy) : base(entity, stateMachine, animBoolName)
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
