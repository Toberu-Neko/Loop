using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordEnhancedAttackState : PlayerSwordAttackState
{
    private int attackCounter;
    private WeaponAttackDetails details;

    public PlayerSwordEnhancedAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = 0;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnKnockback += () => isAttackDone = true;

        details = swordData.enhancedAttackDetails[attackCounter];
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationStartMovement)
        {
            Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
        }
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnKnockback -= () => isAttackDone = true;

        attackCounter++;
        if (attackCounter >= swordData.enhancedAttackDetails.Length)
        {
            attackCounter = 0;
            player.WeaponManager.SetEnhanceSwordAttack(false);
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Sword, details);
    }
    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }
}
