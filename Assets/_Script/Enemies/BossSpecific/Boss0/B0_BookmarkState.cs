using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B0_BookmarkState : EnemyBookmarkState
{
    private Boss0 boss;

    public B0_BookmarkState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, S_EnemyBookmarkState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }
}
