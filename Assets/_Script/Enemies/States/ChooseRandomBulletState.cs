using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomBulletState : EnemyState
{
    private ED_ChooseRandomBulletState stateData;
    protected int bulletIndex;
    private bool gotoFirstState;
    private bool gotoSecondState;
    private bool[] gotoThirdState;
    public ChooseRandomBulletState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_ChooseRandomBulletState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        gotoFirstState = true;
        gotoSecondState = true;

        gotoThirdState = new bool[stateData.getCertainBulletHPPercentage.Length];
        for (int i = 0; i < gotoThirdState.Length; i++)
        {
            gotoThirdState[i] = true;
        }
    }

    public override void Enter()
    {
        base.Enter();

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

        for(int i = 0; i < gotoThirdState.Length; i++)
        {
            if (Stats.Health.CurrentValuePercentage <= stateData.getCertainBulletHPPercentage[i] && gotoThirdState[i])
            {
                Debug.Log(stateData.getCertainBulletHPPercentage[i]);
                gotoThirdState[i] = false;
                bulletIndex = 2;
                entity.Anim.SetInteger("bulletIndex", bulletIndex);
                return;
            }
        }

        float max = 0f;
        foreach(float item in stateData.everyBulletProb)
        {
            max += item;
        }

        float random = Random.Range(0f, max);

        float sum = 0f;

        for (int i = 0; i < stateData.everyBulletProb.Length; i++)
        {
            sum += stateData.everyBulletProb[i];

            if (random <= sum)
            {
                bulletIndex = i;
                entity.Anim.SetInteger("bulletIndex", bulletIndex);
                return;
            }
        }

        Debug.LogError("ChooseRandomBulletState: Random bullet index not found");
        bulletIndex = 0;
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
