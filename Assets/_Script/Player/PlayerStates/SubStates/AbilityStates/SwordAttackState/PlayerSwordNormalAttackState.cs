using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordNormalAttackState : PlayerAbilityState
{
    private int attackCounter;
    private SO_WeaponData_Sword swordData;
    private WeaponAttackDetails details;
    public PlayerSwordNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        swordData = player.SwordData;
        attackCounter = 0;
    }
    public override void Enter()
    {
        base.Enter();

        details = swordData.AttackDetails[attackCounter];
        player.Anim.SetInteger("swordAttackCount", attackCounter);
    }
    public override void Exit()
    {
        base.Exit();

        attackCounter++;
        if(attackCounter >= swordData.AmountOfAttacks)
        {
            attackCounter = 0;
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(details.damageAmount, details.knockbackAngle, details.knockbackForce);
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(details.movementSpeed * Movement.FacingDirection);
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }

}
