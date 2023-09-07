using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_RangedAttackState : RangedAttackState
{
    private Boss0 boss;
    public B0_RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyRangedAttackState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }
}
