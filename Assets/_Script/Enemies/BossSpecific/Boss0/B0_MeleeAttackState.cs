using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_MeleeAttackState : MeleeAttackState
{
    private Boss0 boss;
    public B0_MeleeAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyMeleeAttackState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }
}
