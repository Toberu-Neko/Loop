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

                int attackDoneCount = 0;

                if (!boss.FourSkyAttackState.IsAttackDone)
                    attackDoneCount++;

                if (!boss.SliceRoomAndExplodeState.IsAttackDone)
                    attackDoneCount++;



                switch (attackDoneCount)
                {
                    case 2:
                        switch (Random.Range(0, 1))
                        {
                            case 0:
                                stateMachine.ChangeState(boss.SliceRoomAndExplodeState);
                                break;
                            case 1:
                                stateMachine.ChangeState(boss.FourSkyAttackState);
                                break;
                        }
                        break;
                    case 1:
                        if (!boss.FourSkyAttackState.IsAttackDone)
                        {
                            stateMachine.ChangeState(boss.FourSkyAttackState);
                        }
                        else if (!boss.SliceRoomAndExplodeState.IsAttackDone)
                        {
                            stateMachine.ChangeState(boss.SliceRoomAndExplodeState);
                        }
                        break;
                    case 0:
                        boss.FourSkyAttackState.ResetAttack();
//                     stateMachine.ChangeState(boss.BackToGroundState);
                        break;
                    default:
                        Debug.LogError("B1_FlyingIdleState: attackDoneCount is not 0, 1, 2");
                        break;
                }
            }
            else
            {
                stateMachine.ChangeState(boss.FlyingMovementState);
            }
        }
    }
}
