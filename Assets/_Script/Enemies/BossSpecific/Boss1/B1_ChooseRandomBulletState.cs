using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_ChooseRandomBulletState : ChooseRandomBulletState
{
    private readonly Boss1 boss;

    public B1_ChooseRandomBulletState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_ChooseRandomBulletState stateData, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        switch (bulletIndex)
        {
            case 0:
                stateMachine.ChangeState(boss.BlueRangedAttackState);
                break;
            case 1:
                stateMachine.ChangeState(boss.RedRangedAttackState);
                break;
            case 2:
                boss.transform.position = boss.SkyTeleportPos.position;
                stateMachine.ChangeState(boss.FlyingIdleState);
                break;
            default:
                Debug.LogError("Bullet index out of range");
                stateMachine.ChangeState(boss.PlayerDetectedMoveState);
                break;
        }
    }
}
