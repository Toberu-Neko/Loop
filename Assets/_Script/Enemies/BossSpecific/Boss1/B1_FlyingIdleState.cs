using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_FlyingIdleState : EnemyFlyingIdleState
{
    private Boss1 boss;
    public B1_FlyingIdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoNextState)
        {
            if(boss.FlyingMovementState.RemainMoveCount == 0)
            {
                boss.FlyingMovementState.ResetMoveCount();

                List<EnemySkyAttackBase> unfinishedAttacks = new();

                if (!boss.FourSkyAttackState.isAttackDone)
                    unfinishedAttacks.Add(boss.FourSkyAttackState);

                if (!boss.SliceRoomAndExplodeState.isAttackDone)
                    unfinishedAttacks.Add(boss.SliceRoomAndExplodeState);

                if (!boss.AbovePlayerAttackState.isAttackDone)
                    unfinishedAttacks.Add(boss.AbovePlayerAttackState);


                if (unfinishedAttacks.Count > 0)
                {
                    int randomIndex = Random.Range(0, unfinishedAttacks.Count);
                    stateMachine.ChangeState(unfinishedAttacks[randomIndex]);
                }
            }
            else
            {
                stateMachine.ChangeState(boss.FlyingMovementState);
            }
        }
    }
}
