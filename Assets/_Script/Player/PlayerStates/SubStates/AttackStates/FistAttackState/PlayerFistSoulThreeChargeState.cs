using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistSoulThreeChargeState : PlayerFistAttackState
{
    SO_WeaponData_Fist data;
    private WeaponAttackDetails[] details;
    public PlayerFistSoulThreeChargeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
        details = data.soulThreeAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        player.WeaponManager.ClearCurrentEnergy();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement)
        {
            Movement.SetVelocityX(data.chargeSpeed);
        }

        if(Combat.DetectedDamageables.Count > 0)
        {

        }
        else if(StartTime >= Time.time + data.chargeTime)
        {
            isAttackDone = true;
        }
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();
    }
}
