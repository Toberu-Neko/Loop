using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_FourSkyAttackState : FourSkyAttackState
{
    private Boss1 boss;

    public B1_FourSkyAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FourSkyAttackState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        boss.BossCam.SetLookatPlayer(true);
    }

    public override void Exit()
    {
        base.Exit();

        boss.BossCam.SetLookatPlayer(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAttackDone)
        {
            stateMachine.ChangeState(boss.FlyingIdleState);
        }
    }
}
