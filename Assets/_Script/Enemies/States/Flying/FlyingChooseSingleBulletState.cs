using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingChooseSingleBulletState : EnemyFlyingStateBase
{
    public FlyingChooseSingleBulletState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_ChooseSingleBulletState stateData, Transform spawnPos) : base(entity, stateMachine, animBoolName)
    {
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();
    }
}
