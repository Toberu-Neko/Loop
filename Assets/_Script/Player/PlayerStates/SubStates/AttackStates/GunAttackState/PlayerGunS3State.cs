using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunS3State : PlayerGunAttackState
{
    private SO_WeaponData_Gun data;
    private int count = 0;
    public PlayerGunS3State(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.GunData;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetInvincibleTrue();
        Movement.SetRBKinematic();


        player.WeaponManager.ClearCurrentEnergy();
        Movement.SetVelocityZero();
        count = 0;
    }

    public override void Exit()
    {
        base.Exit();

        Stats.SetInvincibleFalse();
        Movement.SetRBDynamic();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement && !CollisionSenses.SolidCeiling)
        {
            Movement.SetVelocityY(data.s3UpVelocity);
        }
        else
        {
            Movement.SetVelocityZero();
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        if(data.s3AttackDetails.Length > count)
        {
            DoDamageToDamageList(WeaponType.Gun, data.s3AttackDetails[count]);
            count++;
        }
        else
        {
            Debug.LogError("S3AttackDetails is not enough");
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }
}
