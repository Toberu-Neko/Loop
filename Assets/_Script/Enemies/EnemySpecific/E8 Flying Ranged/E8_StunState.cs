using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_StunState : StunState
{
    private Enemy8 enemy;
    public E8_StunState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyStunState stateData, Enemy8 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isStunTimeOver)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
}
