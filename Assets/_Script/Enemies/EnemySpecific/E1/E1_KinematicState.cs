using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_KinematicState : KinematicState
{
    private Enemy1 enemy;
    public E1_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy1 enemy) : base(entity, stateMachine, animBoolName)
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
