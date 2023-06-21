using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSoulOneAttackState : PlayerAbilityState
{
    WeaponAttackDetails deatails;
    public PlayerSwordSoulOneAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        deatails = player.PlayerWeaponManager.SwordData.soulOneAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetPerfectBlockAttackFalse();
        player.PlayerWeaponManager.DecreaseEnergy();
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(deatails.damageAmount, deatails.knockbackAngle, deatails.knockbackForce);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(deatails.movementSpeed * Movement.FacingDirection);
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
    }
}
