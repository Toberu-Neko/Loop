using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordCounterAttackState : PlayerSwordAttackState
{
    public PlayerSwordCounterAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement)
        {
            Movement.SetVelocityX(swordData.counterAttackDetails.movementSpeed * Movement.FacingDirection);
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Sword, swordData.counterAttackDetails.damageAmount, swordData.counterAttackDetails.staminaDamageAmount, swordData.counterAttackDetails.knockbackAngle, swordData.counterAttackDetails.knockbackForce); 
    }
}
