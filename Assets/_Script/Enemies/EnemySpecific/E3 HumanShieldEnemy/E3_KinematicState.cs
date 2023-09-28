using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_KinematicState : KinematicState
{
    private Enemy3 enemy;
    public E3_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy3 enemy) : base(entity, stateMachine, animBoolName)
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
