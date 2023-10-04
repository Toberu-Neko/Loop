using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInitAnimState : EnemyState
{
    public BossInitAnimState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }
}
