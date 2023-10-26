using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_Idle : IdleState
{
    private Boss0New boss0New;
    public B0N_Idle(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Boss0New boss0New) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss0New = boss0New;
    }
}
