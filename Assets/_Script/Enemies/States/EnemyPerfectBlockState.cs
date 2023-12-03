using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerfectBlockState : EnemyState
{
    protected bool gotoNextState;
    protected bool gotoCounterState;
    private ED_EnemyPerfectBlockState stateData;
    private int actionCounter = 0;
    private Transform rangedattackPos;
    public EnemyPerfectBlockState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyPerfectBlockState stateData, Transform rangedattackPos) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.rangedattackPos = rangedattackPos;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
        gotoNextState = false;
        gotoCounterState = false;

        Combat.OnPerfectBlock += Combat_OnPerfectBlock;
        actionCounter = 0;
    }


    public override void Exit()
    {
        base.Exit();

        Combat.OnPerfectBlock -= Combat_OnPerfectBlock;
        Combat.SetPerfectBlock(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        switch (actionCounter)
        {
            case 0:
                if (stateData.perfectObjPrefab)
                {
                    ObjectPoolManager.SpawnObject(stateData.perfectObjPrefab, rangedattackPos.position, Quaternion.identity);
                }
                Combat.SetPerfectBlock(true);
                break;
            case 1:
                Combat.SetPerfectBlock(false);
                break;
            default:
                break;
        }

        actionCounter++;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        gotoNextState = true;
    }

    private void Combat_OnPerfectBlock()
    {
        foreach (var item in Combat.DetectedKnockbackables)
        {
            item.Knockback(stateData.knockbackAngle, stateData.knockbackForce, Movement.ParentTransform.position, false);
        }

        gotoCounterState = true;
    }

    public bool CanChangeState()
    {
        return EndTime == 0f || Time.time >= EndTime + stateData.cooldown;
    }
}
