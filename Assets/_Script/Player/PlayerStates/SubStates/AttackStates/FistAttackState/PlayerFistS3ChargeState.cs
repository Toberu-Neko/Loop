using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistS3ChargeState : PlayerFistAttackState
{
    SO_WeaponData_Fist data;
    private WeaponAttackDetails[] details;
    public PlayerFistS3ChargeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
        details = data.soulThreeAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        player.WeaponManager.ClearCurrentEnergy();
        Stats.SetInvincibleTrue();
    }

    public override void Exit()
    {
        base.Exit();
        Stats.SetInvincibleFalse();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityX(data.s3ChargeSpeed);

        if (Combat.DetectedDamageables.Count > 0 && Combat.DetectedStaminaDamageables.Count > 0)
        {
            Combat.DetectedDamageables[0].GetGameObject().transform.SetParent(player.transform);
            Combat.DetectedStaminaDamageables[0].TakeStaminaDamage(1000, Movement.ParentTransform.position, false);
            stateMachine.ChangeState(player.FistS3AttackState);
        }
        else if(Time.time >= StartTime + data.s3MaxChargeTime)
        {
            isAttackDone = true;
            //TODO: Miss State
        }
    }
}
