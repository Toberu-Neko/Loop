using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkyAttackBase : EnemyFlyingStateBase
{
    public bool isAttackDone = false;
    public bool doRewind = false;
    public EnemySkyAttackBase(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        isAttackDone = false;
        doRewind = false;
    }
    public override void Enter()
    {
        base.Enter();

        doRewind = Stats.IsAngry;
    }
    public void ResetAttack() => isAttackDone = false;
    public void SetDoRewindTrue() => doRewind = true;
}
