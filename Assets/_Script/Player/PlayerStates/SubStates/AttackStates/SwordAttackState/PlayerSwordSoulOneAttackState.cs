using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSoulOneAttackState : PlayerSwordAttackState
{
    WeaponAttackDetails details;
    public PlayerSwordSoulOneAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.PlayerWeaponManager.SwordData.soulOneAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        Stats.SetPerfectBlockAttackFalse();
        player.PlayerWeaponManager.DecreaseEnergy();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement)
        {
            Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Sword, details.damageAmount,details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce, false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
    }

}
