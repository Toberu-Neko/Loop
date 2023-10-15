using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlayerAttackState : EnemyState
{
    private ED_AbovePlayerAttackState stateData;
    public AbovePlayerAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_AbovePlayerAttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
}
