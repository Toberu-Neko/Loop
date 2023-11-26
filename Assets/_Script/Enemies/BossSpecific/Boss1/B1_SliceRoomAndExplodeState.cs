using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_SliceRoomAndExplodeState : SliceRoomAndExplodeState
{
    private Boss1 boss;

    public B1_SliceRoomAndExplodeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_SliceRoomAndExplodeState stateData, BoxCollider2D bossRoom, Transform attackPos, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData, bossRoom, attackPos)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAttackDone)
        {
            stateMachine.ChangeState(boss.AfterMagic);
        }
    }
}
