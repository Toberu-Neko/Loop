using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerfectBlockState : EnemyState
{
    protected bool gotoNextState;
    private ED_EnemyPerfectBlockState stateData;
    public EnemyPerfectBlockState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyPerfectBlockState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnPerfectBlock += Combat_OnPerfectBlock;
        Combat.SetPerfectBlock(true);
        gotoNextState = false;
    }

    private void Combat_OnPerfectBlock()
    {
        foreach (var item in Combat.DetectedKnockbackables)
        {
            item.Knockback(stateData.knockbackAngle, stateData.knockbackForce, Movement.ParentTransform.position, false);
        }

        gotoNextState = true;
        //TODO: Counter!!!!
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnPerfectBlock -= Combat_OnPerfectBlock;
        Combat.SetPerfectBlock(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Combat.SetPerfectBlock(false);
        gotoNextState = true;
    }
}
