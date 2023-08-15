using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_StunState : StunState
{
    private Enemy5 enemy;
    public E5_StunState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyStunState stateData, Enemy5 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
