using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_EnemyPerfectBlockState : EnemyPerfectBlockState
{
    private Boss1 boss;
    private ED_EnemyProjectiles stateData;

    public B1_EnemyPerfectBlockState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyPerfectBlockState stateData, Transform rangedattackPos, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData, rangedattackPos)
    {
        this.boss = boss;
        this.stateData = boss.StateData.counterAttackObjsData;
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySoundFX(boss.StateData.block, Movement.ParentTransform, AudioManager.SoundType.threeD);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (gotoNextState)
        {
            stateMachine.ChangeState(boss.PlayerDetectedMoveState);
        }
        else if (gotoCounterState)
        {
            PasteMagicOnPlayer();
            stateMachine.ChangeState(boss.CounterAttackState);
        }
    }
    private void PasteMagicOnPlayer()
    {
        /*
        int a = 0, b = 0;
        for(int i = 0; i < 100; i++)
        {
            int testr = Random.Range(0, stateData.pasteItems.Length);
            if(testr == 0)
            {
                a++;
            }
            else if(testr == 1)
            {
                b++;
            }


        }

        Debug.Log("0: " + a + " 1: " + b);
        */

        int random = Random.Range(0, stateData.pasteItems.Length);

        if (Combat.DetectedDamageables.Count == 0)
            return;

        Transform player = Combat.DetectedDamageables[0].GetGameObject().transform;

        ObjectPoolManager.SpawnObject(stateData.pasteItems[random], player);
    }
}
