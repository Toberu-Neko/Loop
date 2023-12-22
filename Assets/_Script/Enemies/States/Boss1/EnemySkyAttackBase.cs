using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkyAttackBase : EnemyFlyingStateBase
{
    public bool isAttackDone = false;
    public bool doRewind = false;
    private float startTime;
    public EnemySkyAttackBase(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
        isAttackDone = false;
        doRewind = false;
    }
    public override void Enter()
    {
        base.Enter();

        doRewind = Stats.IsAngry;
        startTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + 10f)
            isAttackDone = true;
    }

    public void ResetAttack() => isAttackDone = false;
    public void SetDoRewindTrue() => doRewind = true;
}
