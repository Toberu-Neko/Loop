using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_KinematicState : KinematicState
{
    private Enemy4 enemy;
    public E4_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy4 enemy) : base(entity, stateMachine, animBoolName)
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
