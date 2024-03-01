using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_KinematicState : KinematicState
{
    private Enemy8 enemy;
    public E8_KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Enemy8 enemy) : base(entity, stateMachine, animBoolName)
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
