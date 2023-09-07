using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_IdleState : IdleState
{
    private Boss0 boss;
    public B0_IdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyIdleState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
}
