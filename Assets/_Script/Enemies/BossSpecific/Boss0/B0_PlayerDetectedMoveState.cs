using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_PlayerDetectedMoveState : PlayerDetectedMoveState
{
    private Boss0 boss;
    public B0_PlayerDetectedMoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_PlayerDetectedMoveState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
}
