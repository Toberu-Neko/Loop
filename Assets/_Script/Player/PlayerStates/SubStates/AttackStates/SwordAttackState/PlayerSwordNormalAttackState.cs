using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordNormalAttackState : PlayerSwordAttackState
{
    private int attackCounter;
    private WeaponAttackDetails details;

    private float lastAttackTime;


    public PlayerSwordNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        attackCounter = 0;
        lastAttackTime = 0;
    }
    public override void Enter()
    {
        base.Enter();

        Combat.OnKnockback += () => isAttackDone = true;

        if (Time.time >= lastAttackTime + swordData.resetAttackTime)
        {
            attackCounter = 0;
        }

        details = swordData.enhancedAttackDetails[attackCounter];
        player.Anim.SetInteger("attackCount", attackCounter);
    }
    public override void Exit()
    {
        base.Exit();

        Combat.OnKnockback -= () => isAttackDone = true;
        attackCounter++;
        lastAttackTime = Time.time;

        if (attackCounter >= swordData.AmountOfAttacks)
        {
            attackCounter = 0;
        }
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
