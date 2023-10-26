using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0N_DeadState : DeadState
{
    private Boss0New boss0New;
    public B0N_DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Boss0New boss0New) : base(entity, stateMachine, animBoolName)
    {
        this.boss0New = boss0New;
    }

    public override void Enter()
    {
        base.Enter();

        Death.Die();
    }
}
