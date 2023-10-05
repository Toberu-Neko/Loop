using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomBulletState : EnemyState
{
    private ED_ChooseRandomBulletState stateData;
    protected int bulletIndex;
    private bool gotoFirstState;
    private bool gotoSecondState;
    public ChooseRandomBulletState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_ChooseRandomBulletState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        gotoFirstState = true;
        gotoSecondState = true;
    }

    public override void Enter()
    {
        base.Enter();

        bulletIndex = Random.Range(0, stateData.randomCount);

        if (gotoFirstState)
        {
            gotoFirstState = false;
            bulletIndex = 0;
            entity.Anim.SetInteger("bulletIndex", bulletIndex);
            return;
        }

        if (gotoSecondState)
        {
            gotoSecondState = false;
            bulletIndex = 1;
            entity.Anim.SetInteger("bulletIndex", bulletIndex);
            return;
        }
        entity.Anim.SetInteger("bulletIndex", bulletIndex);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!CheckPlayerSenses.IsPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }
}
