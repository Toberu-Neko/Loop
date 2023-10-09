using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounterAttackState : EnemyState
{
    private ED_EnemyProjectiles stateData;
    public EnemyCounterAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyProjectiles stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

}
