using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_SliceRoomAndExplodeState : SliceRoomAndExplodeState
{
    private Boss1 boss;

    public B1_SliceRoomAndExplodeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_SliceRoomAndExplodeState stateData, BoxCollider2D bossRoom, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData, bossRoom)
    {
        this.boss = boss;
    }
}
