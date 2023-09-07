using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBookmarkState : EnemyState
{
    protected S_EnemyBookmarkState stateData;
    public EnemyBookmarkState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyBookmarkState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }
}
